using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class BusinessExpenceConfig : IEntityTypeConfiguration<BusinessExpence>
    {
        public void Configure(EntityTypeBuilder<BusinessExpence> builder)
        {
            // 1. Table Name
            builder.ToTable("BusinessExpence");

            // 2. Primary Key (Integer, Not Null)
            builder.HasKey(be => be.Id);
            builder.Property(be => be.Id)
                   .ValueGeneratedOnAdd();

            // 3. Foreign Key: AccountId (Integer, Not Null)
            builder.Property(be => be.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(be => be.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 4. ExpenceType (String)
            builder.Property(be => be.ExpenceType)
                   .HasMaxLength(250);

            // 5. TotalAmount (Decimal, Not Null)
            builder.Property(be => be.TotalAmount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            // 6. Date (Not Null)
            builder.Property(be => be.Date)
                   .IsRequired();

            // 6. BuyingBillId (Nullable FK) — links expense back to its source buying bill
            builder.Property(be => be.BuyingBillId)
                   .IsRequired(false);

            builder.HasOne(be => be.BuyingBill)
                   .WithMany(b => b.Expences)
                   .HasForeignKey(be => be.BuyingBillId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            // 7. Navigation: Payments
            builder.HasMany(be => be.Payments)
                   .WithOne()
                   .HasForeignKey(p => p.BusinessExpenceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}