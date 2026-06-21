using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class SellingBillConfig : IEntityTypeConfiguration<SellingBill>
    {
        public void Configure(EntityTypeBuilder<SellingBill> builder)
        {
            // 1. Table Name
            builder.ToTable("SellingBill");

            // 2. Primary Key
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                   .ValueGeneratedOnAdd();

            // 3. Required Fields & Constraints
            builder.Property(s => s.BillNo)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.Date)
                   .IsRequired();


            // 4. Foreign Key (Account FK)
            builder.Property(s => s.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(s => s.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Customer)
                   .WithMany(c => c.SellingBills)
                   .HasForeignKey(s => s.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 5. Navigation: Items, Payments
            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey(i => i.BillId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Payments)
                   .WithOne()
                   .HasForeignKey(p => p.BillId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}