using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Application.DTOs.User;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 1. Table Name
            builder.ToTable("User");

            // 2. Primary Key
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(u => u.MiddleName)
                   .HasMaxLength(250); // Optional

            builder.Property(u => u.Surname)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(u => u.Email)
                    .IsRequired()
                   .HasMaxLength(250);

            builder.Property(u => u.PhoneNo)
                   .HasMaxLength(50);

            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Password)
                   .IsRequired();

            // 4. Indexes (Performance & Uniqueness)
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            // 5. Default Values (Database Level)
            builder.Property(u => u.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.HasOne(u => u.Account)
                .WithMany()
                .HasForeignKey(u => u.AccountId)
                .OnDelete(DeleteBehavior.SetNull);



            builder.HasData(
                new List<User>() {
                    new User() {
                        Id = -1,
                        FirstName = "System",
                        Surname = "User",
                        Email = "hiteshkumar252020@gmail.com",
                        Role = "SuperAdmin",
                        Password = BCrypt.Net.BCrypt.HashPassword("Hello@123"),
                    }
                }
           );
        }
    }
}