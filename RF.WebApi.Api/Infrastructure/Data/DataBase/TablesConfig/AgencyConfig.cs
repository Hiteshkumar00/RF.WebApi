using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AgencyConfig : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            // 1. Table Name
            builder.ToTable("Agency");

            // 2. Primary Key (Integer, Not Null)
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            // 3. AgencyName (String, Not Null)
            builder.Property(a => a.AgencyName)
                   .IsRequired()
                   .HasMaxLength(250);

            // 4. Address (String)
            builder.Property(a => a.Address)
                   .HasMaxLength(500);

            // 5. Foreign Key: AccountId (Integer, Account FK, Not Null)
            builder.Property(a => a.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(a => a.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}