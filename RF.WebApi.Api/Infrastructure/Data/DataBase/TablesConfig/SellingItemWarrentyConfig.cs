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

            // 3. Foreign Key: SellingBillItem (ItemId)
            builder.Property(w => w.ItemId)
                   .IsRequired();

            builder.HasOne<SellingBillItem>()
                   .WithMany()
                   .HasForeignKey(w => w.ItemId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 4. Foreign Key: SellingBill (BillId)
            builder.Property(w => w.BillId)
                   .IsRequired();

            builder.HasOne<SellingBill>()
                   .WithMany()
                   .HasForeignKey(w => w.BillId)
                   .OnDelete(DeleteBehavior.NoAction); // Avoid multiple cascade paths

        }
    }
}