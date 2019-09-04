using System;
using Todddo.Core.Domain;

namespace Todddo.Models
{
    public class ApiErrorModel
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ApiErrorModel()
        {

        }

        public ApiErrorModel(DomainError error)
        {
            Code = error.ErrorCode.ToString().ToLower();
            Message = error.ErrorMessage;
        }
    }
}
