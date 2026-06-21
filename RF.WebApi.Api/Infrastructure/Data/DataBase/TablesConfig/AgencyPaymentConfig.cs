using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class AgencyPaymentConfig : IEntityTypeConfiguration<AgencyPayment>
    {
        public void Configure(EntityTypeBuilder<AgencyPayment> builder)
        {
            // 1. Table Name
            builder.ToTable("AgencyPayment");

            // 2. Primary Key
            builder.HasKey(ap => ap.Id);
            builder.Property(ap => ap.Id)
                   .ValueGeneratedOnAdd();

            // 3. Properties
            builder.Property(ap => ap.Date)
                   .IsRequired();

            builder.Property(ap => ap.Description)
                   .HasMaxLength(1000);

            // 4. Foreign Key: AccountId (Account FK)
            builder.Property(ap => ap.AccountId)
                   .IsRequired();

            builder.HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(ap => ap.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 5. Foreign Key: AgencyId (Agency FK)
            builder.Property(ap => ap.AgencyId)
                   .IsRequired();

            builder.HasOne(ap => ap.Agency)
                   .WithMany()
                   .HasForeignKey(ap => ap.AgencyId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 6. Navigation: Transactions
            builder.HasMany(ap => ap.Transactions)
                   .WithOne(t => t.AgencyPayment)
                   .HasForeignKey(t => t.AgencyPaymentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
