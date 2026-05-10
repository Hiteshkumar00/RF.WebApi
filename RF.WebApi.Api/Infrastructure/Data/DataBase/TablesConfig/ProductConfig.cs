using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.ProductName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.AccountId)
                   .IsRequired();

            // Unique constraint: Product Name unique per Account
            builder.HasIndex(p => new { p.AccountId, p.ProductName })
                   .IsUnique();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(p => p.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
