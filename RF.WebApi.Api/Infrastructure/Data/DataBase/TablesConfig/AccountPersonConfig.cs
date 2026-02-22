using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AccountPersonConfig : IEntityTypeConfiguration<AccountPerson>
    {
        public void Configure(EntityTypeBuilder<AccountPerson> builder)
        {
            // 1. Table Name
            builder.ToTable("AccountPerson");

            // 2. Primary Key (Integer, Not Null)
            builder.HasKey(ap => ap.Id);
            builder.Property(ap => ap.Id)
                   .ValueGeneratedOnAdd();

            // 3. Required Fields & Max Lengths
            builder.Property(ap => ap.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(ap => ap.PhoneNo)
                   .HasMaxLength(50);

            builder.Property(ap => ap.Email)
                   .HasMaxLength(250);

            builder.Property(ap => ap.PersonOccupation)
                   .HasMaxLength(250);

            builder.Property(ap => ap.Address)
                   .HasMaxLength(500);

            // 4. Foreign Key: AccountId (Account FK, Not Null)
            builder.Property(ap => ap.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(ap => ap.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}