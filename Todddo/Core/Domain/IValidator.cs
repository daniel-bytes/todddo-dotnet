using System;
using System.Threading.Tasks;
using LanguageExt;

namespace Todddo.Core.Domain
{
    public interface IValidator<TID, TEntity>
        where TEntity: IEntity<TID>
    {
        Task<Either<DomainError, TEntity>> Validate(TEntity entity);
    }
}
