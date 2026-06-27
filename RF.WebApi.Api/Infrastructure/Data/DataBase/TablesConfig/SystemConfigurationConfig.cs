using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class SystemConfigurationConfig : IEntityTypeConfiguration<SystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            builder.ToTable("SystemConfiguration");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            builder.Property(s => s.PropertyName).IsRequired().HasMaxLength(250);
            builder.Property(s => s.PropertyDisplayName).IsRequired().HasMaxLength(250);
            builder.Property(s => s.PropertyType).IsRequired().HasMaxLength(50);
            builder.Property(s => s.PropertyValue).IsRequired();

            // Seed initial property
            builder.HasData(
                new SystemConfiguration
                {
                    Id = 1,
                    PropertyName = "EnableDeleteAccount",
                    PropertyDisplayName = "Enable Account Deletion",
                    PropertyType = "boolean",
                    PropertyValue = "false"
                }
            );
        }
    }
}
