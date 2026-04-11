using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class BuyingBillItemConfig : IEntityTypeConfiguration<BuyingBillItem>
    {
        public void Configure(EntityTypeBuilder<BuyingBillItem> builder)
        {
            // 1. Table Name
            builder.ToTable("BuyingBillItem");

            // 2. Primary Key
            builder.HasKey(bi => bi.Id);
            builder.Property(bi => bi.Id)
                   .ValueGeneratedOnAdd();

            // 3. ItemName (String, Not Null)
            builder.Property(bi => bi.ItemName)
                   .IsRequired()
                   .HasMaxLength(500);

            // 4. Quantity (Integer)
            builder.Property(bi => bi.Quantity);

            // 5. Price (Decimal)
            builder.Property(bi => bi.Price)
                   .HasPrecision(18, 2);

            // 6. Foreign Key to BuyingBill
            builder.Property(bi => bi.BillId)
                   .IsRequired();

            builder.HasOne<BuyingBill>()
                   .WithMany(bb => bb.Items)
                   .HasForeignKey(bi => bi.BillId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}