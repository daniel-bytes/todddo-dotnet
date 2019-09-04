using System;
using System.Threading.Tasks;
using LanguageExt;

namespace Todddo.Core.Domain.Todo
{
    public class TodoValidator
        : IValidator<TodoId, TodoEntity>
    {
        public Task<Either<DomainError, TodoEntity>> Validate(TodoEntity entity)
        {
            return Task.Run(() =>
                ValidateTaskNotEmpty(entity)
                    .Bind(ValidateTaskNotTooShort)
                    .Bind(ValidateTaskNotTooLong)
            );
        }

        public Either<DomainError, TodoEntity> ValidateTaskNotEmpty(TodoEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Task))
                return DomainError.FailedValidation("Task cannot be empty");

            return entity;
        }

        public Either<DomainError, TodoEntity> ValidateTaskNotTooShort(TodoEntity entity)
        {
            if ((entity.Task ?? "").Length < 2)
                return DomainError.FailedValidation("Task must be at least 2 characters long");

            return entity;
        }

        public Either<DomainError, TodoEntity> ValidateTaskNotTooLong(TodoEntity entity)
        {
            if ((entity.Task ?? "").Length > 1000)
                return DomainError.FailedValidation("Task must be less than 1000 characters long");

            return entity;
        }
    }
}
