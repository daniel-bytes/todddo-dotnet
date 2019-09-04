using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Todddo.Core.Domain;
using Todddo.Models;

namespace Todddo.Controllers
{
    public static class ControllerExtensions
    {
        public static ActionResult Error(this ControllerBase controller, DomainError error)
        {
            switch(error.ErrorCode)
            {
                case DomainErrorCode.NotFound:
                    return controller.NotFound(ErrorModel(error));
                case DomainErrorCode.FailedValidation:
                    return controller.BadRequest(ErrorModel(error));
                case DomainErrorCode.AlreadyExists:
                    return controller.Conflict(ErrorModel(error));
                default:
                    return controller.StatusCode(500, ErrorModel(error));
            }
        }

        private static ApiErrorModel ErrorModel(DomainError error)
        {
            return new ApiErrorModel
            {
                Code = SnakeCaseErrorCode(error.ErrorCode),
                Message = error.ErrorMessage
            };
        }

        private static string SnakeCaseErrorCode(DomainErrorCode code)
        {
            return String.Concat(
                code.ToString().Select(
                    (c, i) => i > 0 && Char.IsUpper(c) ? ("_" + c.ToString()) : c.ToString()
                )
            ).ToLower();
        }
    }
}
