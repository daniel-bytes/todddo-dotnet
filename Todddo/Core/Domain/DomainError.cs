using System;
namespace Todddo.Core.Domain
{
    public enum DomainErrorCode
    {
        NotFound,
        FailedValidation,
        AlreadyExists
    }

    public class DomainError
    {
        public DomainErrorCode ErrorCode { get; }
        public string ErrorMessage { get; }

        public DomainError(DomainErrorCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public static DomainError NotFound { get; } =
            new DomainError(DomainErrorCode.NotFound, "Entity could not be found");

        public static DomainError AlreadyExists { get; } =
            new DomainError(DomainErrorCode.AlreadyExists, "Entity already exists");

        public static DomainError FailedValidation(string message)
        {
            return new DomainError(DomainErrorCode.FailedValidation, message);
        }
    }
}
