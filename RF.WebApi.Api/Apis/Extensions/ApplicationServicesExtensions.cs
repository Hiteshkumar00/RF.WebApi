using Microsoft.Extensions.DependencyInjection;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Services;

namespace RF.WebApi.Api.Apis.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBusinessYearService, BusinessYearService>();
            services.AddScoped<IAccountPersonService, AccountPersonService>();
            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<IAgencyPersonService, AgencyPersonService>();
            services.AddScoped<IPaymentAccountService, PaymentAccountService>();
            services.AddScoped<IBusinessExpenceService, BusinessExpenceService>();
            services.AddScoped<IBuyingBillService, BuyingBillService>();
            services.AddScoped<ISellingBillService, SellingBillService>();
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<IAddContributionService, AddContributionService>();
            services.AddScoped<IRemoveContributionService, RemoveContributionService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddHttpClient<IWhatsAppIntegrationService, WhatsAppIntegrationService>();

            return services;
        }
    }
}
