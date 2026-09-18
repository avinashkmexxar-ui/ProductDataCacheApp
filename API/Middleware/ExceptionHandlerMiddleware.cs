using Shared.DTOs;

namespace API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception. Occured");
            int code;
            

            switch (exception)
            {
                case HttpRequestException:
                    code = StatusCodes.Status502BadGateway; 
                    break;
                case ArgumentException:
                    code = StatusCodes.Status400BadRequest; 
                    break;
                case KeyNotFoundException:
                    code = StatusCodes.Status404NotFound; 
                    break;
                default:
                    code = StatusCodes.Status500InternalServerError; 
                    break;
            }

            await context.Response.WriteAsJsonAsync(new ResponseDto<object>
            {
                IsSuccess = false,
                Code = code,
                Message = exception.Message,
            });

        }
    }
}
