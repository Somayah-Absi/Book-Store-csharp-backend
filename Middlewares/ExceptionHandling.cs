using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Backend.Middlewares
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UnauthorizedAccessExceptions : Exception
    {
        public UnauthorizedAccessExceptions(string message) : base(message) { }
    }

    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
    public class InternalServerException : Exception
    {
        public InternalServerException(string message) : base(message) { }
    }

    public class ExceptionHandling
    {
        private readonly ILogger<ExceptionHandling> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandling(ILogger<ExceptionHandling> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"---- Exception Handling Requests: {context.Request.Method} {context.Request.Path} ----");
            try
            {
                await _next(context); // controller - service 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled excpetion: {ex}");
                // that will handle specific exception
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                _logger.LogInformation($"---- Finished Exception Handling Requests: Response status {context.Response.StatusCode} ----");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Default response for an unexpected error
            var responseCode = StatusCodes.Status500InternalServerError;
            var message = "An unexpected error has occurred.";

            // Handling specific exceptions
            switch (exception)
            {
                case NotFoundException notFoundException:
                    responseCode = StatusCodes.Status404NotFound;
                    message = notFoundException.Message;
                    break;

                case ValidationException validationException:
                    responseCode = StatusCodes.Status400BadRequest;
                    message = validationException.Message;
                    break;

                case UnauthorizedAccessExceptions unauthorizedAccessException:
                    responseCode = StatusCodes.Status401Unauthorized;
                    message = unauthorizedAccessException.Message;
                    break;

                case ForbiddenAccessException forbiddenAccessException:
                    responseCode = StatusCodes.Status403Forbidden;
                    message = forbiddenAccessException.Message;
                    break;

                case ConflictException conflictException:
                    responseCode = StatusCodes.Status409Conflict;
                    message = conflictException.Message;
                    break;

                case BadRequestException badRequestException:
                    responseCode = StatusCodes.Status400BadRequest;
                    message = badRequestException.Message;
                    break;

                case  InternalServerException internalServerException:
                    responseCode = StatusCodes.Status500InternalServerError;
                    message = internalServerException.Message;
                    break;

                default:
                    // Log the exception if it's not one of the expected types
                    Console.WriteLine("Unhandled exception type: ", exception.GetType());
                    break;
            }

            context.Response.StatusCode = responseCode;

            var response = new
            {
                StatusCode = responseCode,
                Message = message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }

    }
}