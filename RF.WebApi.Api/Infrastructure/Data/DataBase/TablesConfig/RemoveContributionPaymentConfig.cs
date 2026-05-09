using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class RemoveContributionPaymentConfig : IEntityTypeConfiguration<RemoveContributionPayment>
    {
        public void Configure(EntityTypeBuilder<RemoveContributionPayment> builder)
        {
            // 1. Table Name
            builder.ToTable("RemoveContributionPayment");

            // 2. Primary Key
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            // 3. Amount (Decimal, Not Null)
            builder.Property(p => p.Amount)
                   .HasPrecision(18, 2);

            // 4. Foreign Key: RemoveContribution (Integer, Not Null)
            builder.Property(p => p.RemoveContributionId)
                   .IsRequired();            builder.HasOne(p => p.RemoveContribution)
                   .WithMany(c => c.Payments)
                   .HasForeignKey(p => p.RemoveContributionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 5. Foreign Key: PaymentAccount (Integer, Not Null)
            builder.Property(p => p.PaymentAccountId)
                   .IsRequired();

            builder.HasOne(p => p.PaymentAccount)
                   .WithMany()
                   .HasForeignKey(p => p.PaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}