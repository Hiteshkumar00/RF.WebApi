using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.CustomerName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(c => c.PhoneNo)
                   .HasMaxLength(20);

            builder.Property(c => c.Email)
                   .HasMaxLength(250);

            builder.Property(c => c.Address)
                   .HasMaxLength(500);

            builder.Property(c => c.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(c => c.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
