using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RF.WebApi.Api.Infrastructure.Data.Tables;

namespace RF.WebApi.Api.Infrastructure.Data.DataBase.TablesConfig
{
    public class PaymentTransferConfig : IEntityTypeConfiguration<PaymentTransfer>
    {
        public void Configure(EntityTypeBuilder<PaymentTransfer> builder)
        {
            builder.ToTable("PaymentTransfer");

            builder.HasKey(pt => pt.Id);
            builder.Property(pt => pt.Id).ValueGeneratedOnAdd();

            builder.Property(pt => pt.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(pt => pt.Date).IsRequired();
            builder.Property(pt => pt.AccountId).IsRequired();

            builder.HasOne(pt => pt.FromPaymentAccount)
                   .WithMany()
                   .HasForeignKey(pt => pt.FromPaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pt => pt.ToPaymentAccount)
                   .WithMany()
                   .HasForeignKey(pt => pt.ToPaymentAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
