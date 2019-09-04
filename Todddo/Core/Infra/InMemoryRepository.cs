using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using Todddo.Core.Domain;

namespace Todddo.Core.Infra
{
    public abstract class InMemoryRepository<TID, TEntity>: IRepository<TID, TEntity>
        where TEntity: IEntity<TID>
    {
        private readonly IValidator<TID, TEntity> validator;

        public Dictionary<TID, TEntity> Values { get; }

        protected InMemoryRepository(IValidator<TID, TEntity> validator)
            : this(validator, new Dictionary<TID, TEntity>())
        {
        }

        protected InMemoryRepository(IValidator<TID, TEntity> validator, IEnumerable<KeyValuePair<TID, TEntity>> values)
        {
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            Values = new Dictionary<TID, TEntity>(values ?? throw new ArgumentNullException(nameof(values)));
        }

        public Task<Either<DomainError, TEntity>> Get(TID id)
        {
            return Task.Run<Either<DomainError, TEntity>>(() =>
            {
                if (Values.ContainsKey(id))
                {
                    return Values[id];
                }

                return DomainError.NotFound;
            });
        }

        public Task<Either<DomainError, ICollection<TEntity>>> List()
        {
            return Task.Run<Either<DomainError, ICollection<TEntity>>>(() =>
                new List<TEntity>(Values.Values));
        }

        public async Task<Either<DomainError, TEntity>> Create(TEntity entity)
        {
            var validated = await validator.Validate(entity);

            return validated.Bind(CreateValidatedEntity);
        }

        public async Task<Either<DomainError, TEntity>> Update(TEntity entity)
        {
            var validated = await validator.Validate(entity);

            return validated.Bind(UpdateValidatedEntity);
        }

        public Task<Either<DomainError, TEntity>> Delete(TID id)
        {
            return Task.Run<Either<DomainError, TEntity>>(() =>
            {
                if (Values.ContainsKey(id))
                {
                    var data = Values[id];
                    Values.Remove(id);
                    return data;
                }

                return DomainError.NotFound;
            });
        }

        private Either<DomainError, TEntity> CreateValidatedEntity(TEntity entity)
        {
            if (Values.ContainsKey(entity.Id))
            {
                return DomainError.AlreadyExists;
            }

            Values.Add(entity.Id, entity);
            return entity;
        }

        private Either<DomainError, TEntity> UpdateValidatedEntity(TEntity entity)
        {
            if (Values.ContainsKey(entity.Id))
            {
                Values[entity.Id] = entity;
                return entity;
            }

            return DomainError.NotFound;
        }
    }
}
