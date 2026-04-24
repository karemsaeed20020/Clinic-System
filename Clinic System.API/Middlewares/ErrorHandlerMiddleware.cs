
using Clinic_System.Application.Common.Bases;

namespace Clinic_System.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("ErrorHandlerMiddleware Invoked.");
            try
            {
                _logger.LogInformation("Processing Request.");
                await _next(context);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An error occurred while processing the request.");
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new Response<string>()
                {
                    Succeeded = false,
                    Message = error.Message
                };
            }
        }
    }
}
