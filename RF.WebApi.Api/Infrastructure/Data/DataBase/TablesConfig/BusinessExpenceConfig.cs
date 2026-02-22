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
            // Linked to Account as per the "SellingBill FK" requirement
            builder.Property(be => be.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(be => be.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 4. ExpenceType (String)
            builder.Property(be => be.ExpenceType)
                   .HasMaxLength(250);

            // 5. Date (String, Not Null)
            builder.Property(be => be.Date)
                   .IsRequired();
        }
    }
}