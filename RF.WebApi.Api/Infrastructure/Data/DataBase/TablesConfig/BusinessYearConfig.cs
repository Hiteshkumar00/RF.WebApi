using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class BusinessYearConfig : IEntityTypeConfiguration<BusinessYear>
    {
        public void Configure(EntityTypeBuilder<BusinessYear> builder)
        {
            // 1. Table Name
            builder.ToTable("BusinessYear");

            // 2. Primary Key
            builder.HasKey(by => by.Id);
            builder.Property(by => by.Id)
                   .ValueGeneratedOnAdd();

            // 3. Required Fields & Constraints
            builder.Property(by => by.YearName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(by => by.Date)
                   .IsRequired()
                   .HasMaxLength(50);

            // 4. Foreign Key: AccountId (Account FK)
            builder.Property(by => by.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(by => by.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}