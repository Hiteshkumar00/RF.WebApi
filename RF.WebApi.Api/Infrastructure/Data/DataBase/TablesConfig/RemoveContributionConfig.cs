using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class RemoveContributionConfig : IEntityTypeConfiguration<RemoveContribution>
    {
        public void Configure(EntityTypeBuilder<RemoveContribution> builder)
        {
            // 1. Table Name
            builder.ToTable("RemoveContribution");

            // 2. Primary Key
            builder.HasKey(rc => rc.Id);
            builder.Property(rc => rc.Id)
                   .ValueGeneratedOnAdd();

            // 3. Foreign Key: AccountPersonId (Integer, Not Null)
            builder.Property(rc => rc.AccountPersonId);

            builder.Property(rc => rc.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(rc => rc.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rc => rc.AccountPerson)
                   .WithMany()
                   .HasForeignKey(rc => rc.AccountPersonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 4. Description (String)
            builder.Property(rc => rc.Description)
                   .HasMaxLength(500);

            // 5. Date (String, Not Null)
            builder.Property(rc => rc.Date)
                   .IsRequired();

            // 6. Navigation
            builder.HasMany(rc => rc.Payments)
                   .WithOne()
                   .HasForeignKey(p => p.RemoveContributionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
