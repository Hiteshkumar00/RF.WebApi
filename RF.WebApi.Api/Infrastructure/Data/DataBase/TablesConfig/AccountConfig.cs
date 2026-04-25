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
            
            builder.Property(a => a.Title)
                   .HasMaxLength(250)
                   .IsRequired(false);

            builder.Property(a => a.Address)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(a => a.Phone)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(a => a.Email)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(a => a.GSTIN)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(a => a.CurrencyType)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("INR");

            builder.Property(a => a.DateFormat)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(a => a.ShortDateFormat)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(a => a.EnableSuggestions)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(a => a.WhatsAppNumber)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(a => a.EnableWhatsApp)
                   .IsRequired()
                   .HasDefaultValue(false);
        }
    }
}
