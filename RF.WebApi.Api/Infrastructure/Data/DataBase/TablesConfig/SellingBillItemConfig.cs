using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class SellingBillItemConfig : IEntityTypeConfiguration<SellingBillItem>
    {
        public void Configure(EntityTypeBuilder<SellingBillItem> builder)
        {
            // 1. Table Name
            builder.ToTable("SellingBillItem");

            // 2. Primary Key (Integer, Not Null)
            builder.HasKey(sbi => sbi.Id);
            builder.Property(sbi => sbi.Id)
                   .ValueGeneratedOnAdd();

            // 3. ItemName (String, Not Null)
            builder.Property(sbi => sbi.ItemName)
                   .IsRequired()
                   .HasMaxLength(500);

            // 4. Quantity (Integer)
            builder.Property(sbi => sbi.Quantity)
                   .IsRequired();

            // 5. Price (Decimal)
            builder.Property(sbi => sbi.Price)
                   .HasPrecision(18, 2);

            // 6. Relationship to SellingBill
            builder.Property(sbi => sbi.BillId)
                   .IsRequired();

            builder.HasOne<SellingBill>()
                   .WithMany(sb => sb.Items)
                   .HasForeignKey(sbi => sbi.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 7. Navigation: 1-to-1 Warranty
            builder.HasOne(sbi => sbi.Warrenty)
                   .WithOne()
                   .HasForeignKey<SellingItemWarrenty>(w => w.ItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}