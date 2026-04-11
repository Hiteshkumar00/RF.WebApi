using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class BuyingBillPaymentConfig : IEntityTypeConfiguration<BuyingBillPayment>
    {
        public void Configure(EntityTypeBuilder<BuyingBillPayment> builder)
        {
            // 1. Table Name
            builder.ToTable("BuyingBillPayment");

            // 2. Primary Key
            builder.HasKey(bbp => bbp.Id);
            builder.Property(bbp => bbp.Id)
                   .ValueGeneratedOnAdd();

            // 3. Amount (Decimal, Not Null)
            builder.Property(bbp => bbp.Amount)
                   .HasPrecision(18, 2);

            // 4. BuyingBill FK (Integer, Not Null)
            builder.Property(bbp => bbp.BillId)
                   .IsRequired();

            builder.HasOne<BuyingBill>()
                   .WithMany(bb => bb.Payments)
                   .HasForeignKey(bbp => bbp.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 5. PaymentAccount FK (Integer, Not Null)
            builder.Property(bbp => bbp.PaymentAccountId)
                   .IsRequired();

            builder.HasOne<PaymentAccount>()
                   .WithMany()
                   .HasForeignKey(bbp => bbp.PaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
