using Store.Route.Domain.Exceptions;
using Store.Route.Shared.ErrorModel;

namespace Store.Route.Web.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next,ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPoint(context);
                }
            }
            catch (Exception ex)
            {
                //Log expression
                _logger.LogError(ex, ex.Message);

                //1.set statusCode of response
                //2.set content-type of response
                //3.set body of response
                //4.return Response
                //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await HandleError(context, ex);
            }

        }

        private static async Task HandleError(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var Response = new ErrorDetails()
            {
                //StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMessage = ex.Message
            };
            Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnAuthorizedException=>StatusCodes.Status401Unauthorized,
                ValidationException=>HandleValidationErrorException((ValidationException)ex,Response),
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = Response.StatusCode;
            await context.Response.WriteAsJsonAsync(Response);
        }

        private static async Task HandlingNotFoundEndPoint(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var Response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End point {context.Request.Path} is not found"
            };
            await context.Response.WriteAsJsonAsync(Response);
        }

        private static int HandleValidationErrorException(ValidationException ex,ErrorDetails response)
        {
            response.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;

        }
    }
}
