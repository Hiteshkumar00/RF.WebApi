using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace RF.WebApi.Api.Apis.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var response = new
                        {
                            Success = false,
                            Data = (object)null,
                            Message = $"Internal Server Error | {contextFeature.Error.Message}"
                        };

                        await context.Response.WriteAsJsonAsync(response);
                    }
                });
            });
        }
    }
}
