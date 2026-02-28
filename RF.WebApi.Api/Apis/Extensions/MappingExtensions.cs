using RF.WebApi.Api.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace RF.WebApi.Api.Apis.Extensions
{
    public static class MappingExtensions
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            //Addmore profiles here as you create them, e.g.:
        }
    }
}