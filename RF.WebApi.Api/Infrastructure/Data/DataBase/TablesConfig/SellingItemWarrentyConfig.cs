using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class SellingItemWarrentyConfig : IEntityTypeConfiguration<SellingItemWarrenty>
    {
        public void Configure(EntityTypeBuilder<SellingItemWarrenty> builder)
        {
            // 1. Table Name
            builder.ToTable("SellingItemWarrenty");

            // 2. Primary Key
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Id)
                   .ValueGeneratedOnAdd();

            // 3. Relationship: SellingBillItem (ItemId)
            builder.Property(w => w.ItemId)
                   .IsRequired();

            builder.HasOne(w => w.Item)
                   .WithOne(i => i.Warrenty)
                   .HasForeignKey<SellingItemWarrenty>(w => w.ItemId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 4. Relationship: SellingBill (BillId)
            builder.Property(w => w.BillId)
                   .IsRequired();

            builder.HasOne(w => w.Bill)
                   .WithMany()
                   .HasForeignKey(w => w.BillId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}