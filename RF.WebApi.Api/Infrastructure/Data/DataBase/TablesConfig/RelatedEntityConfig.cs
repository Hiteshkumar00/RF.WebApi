using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class RelatedEntityConfig : IEntityTypeConfiguration<RelatedEntity>
    {
        public void Configure(EntityTypeBuilder<RelatedEntity> builder)
        {
            // 1. Table Name
            builder.ToTable("RelatedEntity");

            // 2. Primary Key
            builder.HasKey(re => re.Id);
            builder.Property(re => re.Id)
                   .ValueGeneratedOnAdd();

            // 3. Properties & Constraints
            builder.Property(re => re.RelatedEntityName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(re => re.RelatedDisplayName)
                   .HasMaxLength(250);

            // 4. Foreign Key (Mapping EntityId to User Table)
            builder.Property(re => re.EntityId)
                   .IsRequired();

            builder.HasOne<Entity>()
                   .WithMany()
                   .HasForeignKey(re => re.EntityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}