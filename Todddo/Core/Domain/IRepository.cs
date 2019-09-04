using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;

namespace Todddo.Core.Domain
{
    public interface IRepository<TID, TEntity>
        where TEntity: IEntity<TID>
    {
        Task<Either<DomainError, TEntity>> Get(TID id);
        Task<Either<DomainError, ICollection<TEntity>>> List();
        Task<Either<DomainError, TEntity>> Create(TEntity entity);
        Task<Either<DomainError, TEntity>> Update(TEntity entity);
        Task<Either<DomainError, TEntity>> Delete(TID id);
    }
}
