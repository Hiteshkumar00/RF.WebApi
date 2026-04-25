using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class UserSelectedYearMappingConfig : IEntityTypeConfiguration<UserSelectedYearMapping>
    {
        public void Configure(EntityTypeBuilder<UserSelectedYearMapping> builder)
        {
            builder.ToTable("UserSelectedYearMapping");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(ap => ap.AccountId).IsRequired();
            builder.Property(ap => ap.UserId).IsRequired();
            builder.Property(ap => ap.BusinessYearId).IsRequired();

            // Foreign Key: Account
            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(ap => ap.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Foreign Key: User
            builder.HasOne<User>() 
                   .WithMany()
                   .HasForeignKey(ap => ap.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Foreign Key: BusinessYear
            builder.HasOne<BusinessYear>()
                   .WithMany()
                   .HasForeignKey(ap => ap.BusinessYearId)
                   .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
