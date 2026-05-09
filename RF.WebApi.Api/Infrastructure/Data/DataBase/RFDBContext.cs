using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Infrastructure.Data.Tables;
using RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig;

namespace RF.WebApi.Infrastructure.Data.DataBase
{
    public class RFDBContext : DbContext
    {
        public RFDBContext(DbContextOptions<RFDBContext> options) : base(options)
        {
        }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Entity> Entitys { get; set; }
        public DbSet<RelatedEntity> RelatedEntitys { get; set; }
        public DbSet<PaymentAccount> PaymentAccounts { get; set; }
        public DbSet<SellingBill> SellingBills { get; set; }
        public DbSet<SellingBillPayment> SellingBillPayments { get; set; }
        public DbSet<SellingBillItem> SellingBillItems { get; set; }
        public DbSet<SellingItemWarrenty> SellingItemWarrenties { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencyPerson> AgencyPersons { get; set; }
        public DbSet<BuyingBill> BuyingBills { get; set; }
        public DbSet<BuyingBillItem> BuyingBillItems { get; set; }
        public DbSet<BuyingBillPayment> BuyingBillPayments { get; set; }
        public DbSet<BusinessYear> BusinessYears { get; set; }
        public DbSet<AccountPerson> AccountPersons { get; set; }
        public DbSet<BusinessExpence> BusinessExpences { get; set; }
        public DbSet<BusinessExpencePayment> BusinessExpencePayments { get; set; }
        public DbSet<AddContribution> AddContributions { get; set; }
        public DbSet<AddContributionPayment> AddContributionPayments { get; set; }
        public DbSet<RemoveContribution> RemoveContributions { get; set; }
        public DbSet<RemoveContributionPayment> RemoveContributionPayments { get; set; }
        public DbSet<UserSelectedYearMapping> UserSelectedYearMappings { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply existing configurations...
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new AccountConfig());
            modelBuilder.ApplyConfiguration(new EntityConfig());
            modelBuilder.ApplyConfiguration(new RelatedEntityConfig());
            modelBuilder.ApplyConfiguration(new PaymentAccountConfig());
            modelBuilder.ApplyConfiguration(new SellingBillConfig());
            modelBuilder.ApplyConfiguration(new SellingBillPaymentConfig());
            modelBuilder.ApplyConfiguration(new SellingBillItemConfig());
            modelBuilder.ApplyConfiguration(new SellingItemWarrentyConfig());
            modelBuilder.ApplyConfiguration(new AgencyConfig());
            modelBuilder.ApplyConfiguration(new AgencyPersonConfig());
            modelBuilder.ApplyConfiguration(new BuyingBillConfig());
            modelBuilder.ApplyConfiguration(new BuyingBillItemConfig());
            modelBuilder.ApplyConfiguration(new BuyingBillPaymentConfig());
            modelBuilder.ApplyConfiguration(new BusinessYearConfig());
            modelBuilder.ApplyConfiguration(new AccountPersonConfig());
            modelBuilder.ApplyConfiguration(new BusinessExpenceConfig());
            modelBuilder.ApplyConfiguration(new BusinessExpencePaymentConfig());
            modelBuilder.ApplyConfiguration(new AddContributionConfig());
            modelBuilder.ApplyConfiguration(new AddContributionPaymentConfig());
            modelBuilder.ApplyConfiguration(new RemoveContributionConfig());
            modelBuilder.ApplyConfiguration(new RemoveContributionPaymentConfig());
            modelBuilder.ApplyConfiguration(new UserSelectedYearMappingConfig());
            modelBuilder.ApplyConfiguration(new SystemConfigurationConfig());
        }
    }
}