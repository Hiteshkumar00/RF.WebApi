using Microsoft.AspNetCore.Mvc;

namespace RF.WebApi.Api.Apis.Extensions
{
    public static class ValidationExtensions
    {
        public static void ConfigureCustomValidationErrorResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    // Get all validation errors from the DTO attributes
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => string.IsNullOrEmpty(e.ErrorMessage)
                                    ? e.Exception?.Message
                                    : e.ErrorMessage)
                        .ToList();

                    // Create the custom response matching your Success | Data | Message style
                    var response = new
                    {
                        Success = false,
                        Data = (object)null,
                        // Join them with the | separator as requested
                        Message = string.Join(" | ", errors)
                    };

                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
