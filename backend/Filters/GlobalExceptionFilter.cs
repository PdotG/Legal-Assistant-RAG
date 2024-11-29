using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace backend.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                UnauthorizedAccessException =>
                    new ObjectResult(new { message = "Access denied. Unauthorized." })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    },

                InvalidOperationException =>
                    new BadRequestObjectResult(new { message = "Invalid operation performed." }),

                _ => new ObjectResult(new { message = "An unexpected error occurred." })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                }
            };

            context.ExceptionHandled = true;
        }
    }
}
