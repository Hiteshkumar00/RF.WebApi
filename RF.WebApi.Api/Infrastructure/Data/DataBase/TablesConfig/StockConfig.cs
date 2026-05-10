using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class StockConfig : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stock");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            builder.Property(s => s.BuyingBillId).IsRequired();
            builder.Property(s => s.ProductId).IsRequired();
            builder.Property(s => s.Quantity).IsRequired();
            builder.Property(s => s.PurchasePrice)
                   .IsRequired()
                   .HasPrecision(18, 2);
            builder.Property(s => s.Discount).HasPrecision(18, 2);

            builder.HasOne(s => s.BuyingBill)
                   .WithMany(b => b.Stocks)
                   .HasForeignKey(s => s.BuyingBillId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Product)
                   .WithMany()
                   .HasForeignKey(s => s.ProductId)
                   .OnDelete(DeleteBehavior.NoAction); // Don't delete stock if product is deleted? Or maybe cascade?
                                                     // User didn't specify. Usually NoAction or Cascade.
        }
    }
}
