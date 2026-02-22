using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class PaymentAccountConfig : IEntityTypeConfiguration<PaymentAccount>
    {
        public void Configure(EntityTypeBuilder<PaymentAccount> builder)
        {
            // 1. Table Name
            builder.ToTable("PaymentAccount");

            // 2. Primary Key (Integer, Not Null)
            builder.HasKey(pa => pa.Id);
            builder.Property(pa => pa.Id)
                   .ValueGeneratedOnAdd();

            // 3. MethodName (String, Not Null)
            builder.Property(pa => pa.MethodName)
                   .IsRequired()
                   .HasMaxLength(250);

            // 4. Account FK (Integer, Not Null)
            builder.Property(pa => pa.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(pa => pa.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 5. AccountPerson FK (Integer)
            // Note: Not marked "Not Null" in the image, so it is optional
            builder.HasOne<AccountPerson>() // Assuming AccountPerson entity exists
                   .WithMany()
                   .HasForeignKey(pa => pa.AccountPersonId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
