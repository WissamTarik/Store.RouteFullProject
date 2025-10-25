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
                    context.Response.ContentType = "application/json";

                    var Response = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = $"End point {context.Request.Path} is not found"
                    };
                    await context.Response.WriteAsJsonAsync(Response);
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
                context.Response.ContentType = "application/json";

                var Response = new ErrorDetails()
                {
                    //StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };
                Response.StatusCode = ex switch
                {
                    NotFoundException=>StatusCodes.Status404NotFound,
                    _=>StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = Response.StatusCode;
                  await  context.Response.WriteAsJsonAsync(Response);
            }
            
        }
    }
}
