using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AddContributionPaymentConfig : IEntityTypeConfiguration<AddContributionPayment>
    {
        public void Configure(EntityTypeBuilder<AddContributionPayment> builder)
        {
            // 1. Table Name
            builder.ToTable("AddContributionPayment");

            // 2. Primary Key
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            // 3. Amount (Decimal, Not Null)
            builder.Property(p => p.Amount)
                   .HasPrecision(18, 2);

            // 4. Foreign Key: AddContribution (Integer, Not Null)
            builder.Property(p => p.AddContributionId)
                   .IsRequired();



            // 5. Foreign Key: PaymentAccount (Integer, Not Null)
            builder.Property(p => p.PaymentAccountId)
                   .IsRequired();

            builder.HasOne<PaymentAccount>()
                   .WithMany()
                   .HasForeignKey(p => p.PaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
