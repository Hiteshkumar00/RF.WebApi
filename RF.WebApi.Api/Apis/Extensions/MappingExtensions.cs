using RF.WebApi.Api.Application.Mappings;

namespace RF.WebApi.Api.Apis.Extensions
{
    public static class MappingExtensions
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<SellingBillProfile>();
                cfg.AddProfile<AgencyProfile>();
                cfg.AddProfile<AgencyPersonProfile>();
                cfg.AddProfile<AccountProfile>();
                cfg.AddProfile<AccountPersonProfile>();
                cfg.AddProfile<BusinessExpenceProfile>();
                cfg.AddProfile<BusinessYearProfile>();
                cfg.AddProfile<BuyingBillProfile>();
                cfg.AddProfile<ContributionProfile>();
                cfg.AddProfile<EntityProfile>();
                cfg.AddProfile<PaymentAccountProfile>();

                // Add more profiles here as you create them
            });
        }
    }
}