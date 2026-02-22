using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Set Table Name
            builder.ToTable("Account");

            // Id: Integer, Primary Key, Not Null
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.ProfileName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(a => a.ProfileLogoLink)
                   .IsRequired(false);

            builder.Property(a => a.CurrencyType)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("INR");
        }
    }
}
