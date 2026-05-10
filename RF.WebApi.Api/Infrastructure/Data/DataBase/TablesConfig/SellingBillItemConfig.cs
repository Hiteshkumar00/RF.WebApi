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

            // 3. ProductId (Integer, Not Null)
            builder.Property(sbi => sbi.ProductId)
                   .IsRequired();

            builder.HasOne(sbi => sbi.Product)
                   .WithMany()
                   .HasForeignKey(sbi => sbi.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);

            // 4. Quantity (Integer)
            builder.Property(sbi => sbi.Quantity)
                   .IsRequired();

            // 5. Price (Decimal)
            builder.Property(sbi => sbi.Price)
                   .HasPrecision(18, 2);
            builder.Property(sbi => sbi.Discount)
                   .HasPrecision(18, 2);

            // 6. Relationship to SellingBill
            builder.Property(sbi => sbi.BillId)
                   .IsRequired();

            builder.HasOne(sbi => sbi.Bill)
                   .WithMany(sb => sb.Items)
                   .HasForeignKey(sbi => sbi.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}