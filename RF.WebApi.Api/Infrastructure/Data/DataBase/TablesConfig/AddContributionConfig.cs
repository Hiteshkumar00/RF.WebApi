using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AddContributionConfig : IEntityTypeConfiguration<AddContribution>
    {
        public void Configure(EntityTypeBuilder<AddContribution> builder)
        {
            // 1. Table Name
            builder.ToTable("AddContribution");

            // 2. Primary Key
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .ValueGeneratedOnAdd();

            // 3. Required Fields & Max Lengths
            builder.Property(c => c.Date)
                   .IsRequired();

            builder.Property(c => c.Description)
                   .HasMaxLength(500);

            // 4. Foreign Key: AccountPersonId (AccountPerson FK)
            builder.Property(c => c.AccountPersonId);

            builder.Property(c => c.AccountId)
                   .IsRequired();

            builder.HasOne(c => c.AccountPerson)
                   .WithMany()
                   .HasForeignKey(c => c.AccountPersonId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 5. Navigation
            builder.HasMany(c => c.Payments)
                   .WithOne()
                   .HasForeignKey(p => p.AddContributionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}