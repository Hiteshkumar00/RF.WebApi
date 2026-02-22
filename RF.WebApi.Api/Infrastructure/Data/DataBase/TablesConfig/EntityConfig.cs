using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class EntityConfig : IEntityTypeConfiguration<Entity>
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            // 1. Table Name
            builder.ToTable("Entity");

            // 2. Primary Key (Integer, Not Null)
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

            // 3. EntityName (String, Unique, Not Null)
            builder.Property(e => e.EntityName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.HasIndex(e => e.EntityName)
                   .IsUnique();

            // 4. DisplayName (String)
            builder.Property(e => e.DisplayName)
                   .HasMaxLength(250);
        }
    }
}