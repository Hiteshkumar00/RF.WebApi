using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class BuyingBillExpenceConfig : IEntityTypeConfiguration<BuyingBillExpence>
    {
        public void Configure(EntityTypeBuilder<BuyingBillExpence> builder)
        {
            // 1. Table Name
            builder.ToTable("BuyingBillExpence");

            // 2. Primary Key
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

            // 3. Amount (Decimal)
            builder.Property(e => e.Amount)
                   .HasPrecision(18, 2);

            // 4. ExpenceType (String)
            builder.Property(e => e.ExpenceType)
                   .HasMaxLength(250);

            // 5. BillId FK (Integer, Not Null)
            // Note: Image specifies "SellingBill FK", but in this context, 
            // it likely refers to the BuyingBill being processed.
            builder.Property(e => e.BillId)
                   .IsRequired();

            builder.HasOne<BuyingBill>()
                   .WithMany(bb => bb.Expences)
                   .HasForeignKey(e => e.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 6. PaymentAccount FK (Integer, Not Null)
            builder.Property(e => e.PaymentAccountId)
                   .IsRequired();

            builder.HasOne<PaymentAccount>()
                   .WithMany()
                   .HasForeignKey(e => e.PaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}