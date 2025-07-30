using LibraryManagement.Application.Exceptions;

namespace LibraryManagement.Api.Middlewares
{
    public class ValidationExceptionMiddleware
    {

        #region Fields

        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionMiddleware> _logger;

        #endregion

        #region Constructor


        public ValidationExceptionMiddleware(RequestDelegate next,
            ILogger<ValidationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // we made this middleware to shape the response - not showing validartion's default error response
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Validation failed",
                    errors = errors
                });
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "An unexpected error occurred"
                });
            }
        }

        #endregion
    }
}
