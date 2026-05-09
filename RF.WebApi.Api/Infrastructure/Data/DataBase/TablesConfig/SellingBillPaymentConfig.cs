using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class SellingBillPaymentConfig : IEntityTypeConfiguration<SellingBillPayment>
    {
        public void Configure(EntityTypeBuilder<SellingBillPayment> builder)
        {
            // 1. Table Name
            builder.ToTable("SellingBillPayment");

            // 2. Primary Key
            builder.HasKey(sbp => sbp.Id);
            builder.Property(sbp => sbp.Id)
                   .ValueGeneratedOnAdd();

            // 3. Amount (Decimal)
            builder.Property(sbp => sbp.Amount)
                   .HasPrecision(18, 2); // Standard precision for financial data

            // 4. SellingBill FK (Integer, Not Null)
            builder.Property(sbp => sbp.BillId)
                   .IsRequired();

            builder.HasOne(sbp => sbp.Bill)
                   .WithMany(sb => sb.Payments)
                   .HasForeignKey(sbp => sbp.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 5. PaymentAccount FK (Integer, Not Null)
            builder.Property(sbp => sbp.PaymentAccountId)
                   .IsRequired();

            builder.HasOne(sbp => sbp.PaymentAccount)
                   .WithMany()
                   .HasForeignKey(sbp => sbp.PaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}