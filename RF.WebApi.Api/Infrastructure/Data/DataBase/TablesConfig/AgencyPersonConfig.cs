using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AgencyPersonConfig : IEntityTypeConfiguration<AgencyPerson>
    {
        public void Configure(EntityTypeBuilder<AgencyPerson> builder)
        {
            // 1. Table Name
            builder.ToTable("AgencyPerson");

            // 2. Primary Key
            builder.HasKey(ap => ap.Id);
            builder.Property(ap => ap.Id)
                   .ValueGeneratedOnAdd();

            // 3. Required Fields & Max Lengths
            builder.Property(ap => ap.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(ap => ap.Email)
                   .HasMaxLength(250);

            builder.Property(ap => ap.PhoneNo)
                   .HasMaxLength(50);

            builder.Property(ap => ap.PersonOccupation)
                   .HasMaxLength(250);

            builder.Property(ap => ap.Address)
                   .HasMaxLength(500);

            // 4. Foreign Key: AgencyId (Integer, Agency FK, Not Null)
            builder.Property(ap => ap.AgencyId)
                   .IsRequired();

            builder.HasOne<Agency>()
                   .WithMany()
                   .HasForeignKey(ap => ap.AgencyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}