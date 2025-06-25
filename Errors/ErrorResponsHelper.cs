// Helpers/ErrorResponseHelper.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding; // 이거 꼭 필요!


namespace RigidboysAPI.Errors
{
    public static class ErrorResponseHelper
    {
        public static IActionResult HandleConflict(string code, string message)
        {
            return new ConflictObjectResult(new
            {
                code,
                message
            });
        }

        public static IActionResult HandleServerError(string message, string detail)
        {
            return new ObjectResult(new
            {
                code = ErrorCodes.SERVER_ERROR,
                message,
                detail
            })
            {
                StatusCode = 500
            };
        }

        public static IActionResult HandleBadRequest(ModelStateDictionary modelState)
        {
            return new BadRequestObjectResult(new
            {
                code = ErrorCodes.INVALID_INPUT,
                message = ErrorCodes.INVALID_INPUT_MESSAGE,
                errors = modelState
            });
        }
        public static IActionResult HandleBadRequest(string code, string message)
        {
            return new BadRequestObjectResult(new
            {
                code,
                message
            });
        }

        public static IActionResult HandleNotFound(string code, string message)
        {
            return new NotFoundObjectResult(new
            {
                code,
                message
            });
        }

    }
}
