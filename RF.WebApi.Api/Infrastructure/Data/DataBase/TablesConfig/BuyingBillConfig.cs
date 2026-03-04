using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class BuyingBillConfig : IEntityTypeConfiguration<BuyingBill>
    {
        public void Configure(EntityTypeBuilder<BuyingBill> builder)
        {
            // 1. Table Name
            builder.ToTable("BuyingBill");

            // 2. Primary Key
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                   .ValueGeneratedOnAdd();

            // 3. Properties & Constraints
            builder.Property(b => b.BillNo)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.Date)
                   .IsRequired();

            builder.Property(b => b.Discount)
                   .HasPrecision(18, 2);

            // 4. Foreign Key: AccountId (Account FK)
            builder.Property(b => b.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(b => b.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 5. Foreign Key: AgencyId (Agency FK)
            builder.Property(b => b.AgencyId)
                   .IsRequired();

            builder.HasOne(b => b.Agency)
                   .WithMany()
                   .HasForeignKey(b => b.AgencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 6. Navigation: Items, Payments, Expences
            builder.HasMany(b => b.Items)
                   .WithOne()
                   .HasForeignKey(i => i.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Payments)
                   .WithOne()
                   .HasForeignKey(p => p.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Expences)
                   .WithOne()
                   .HasForeignKey(e => e.BillId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}