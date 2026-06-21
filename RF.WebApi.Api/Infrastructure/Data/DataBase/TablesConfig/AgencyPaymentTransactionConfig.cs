using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AgencyPaymentTransactionConfig : IEntityTypeConfiguration<AgencyPaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<AgencyPaymentTransaction> builder)
        {
            // 1. Table Name
            builder.ToTable("AgencyPaymentTransaction");

            // 2. Primary Key
            builder.HasKey(apt => apt.Id);
            builder.Property(apt => apt.Id)
                   .ValueGeneratedOnAdd();

            // 3. Amount (Decimal, Not Null)
            builder.Property(apt => apt.Amount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            // 4. AgencyPayment FK
            builder.Property(apt => apt.AgencyPaymentId)
                   .IsRequired();

            builder.HasOne(apt => apt.AgencyPayment)
                   .WithMany(ap => ap.Transactions)
                   .HasForeignKey(apt => apt.AgencyPaymentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 5. PaymentAccount FK
            builder.Property(apt => apt.PaymentAccountId)
                   .IsRequired();

            builder.HasOne(apt => apt.PaymentAccount)
                   .WithMany()
                   .HasForeignKey(apt => apt.PaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
