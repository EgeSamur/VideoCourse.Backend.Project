using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;
public class PaymentConfiguration : BaseEntityConfiguration<Payment>
{
    public override void Configure(EntityTypeBuilder<Payment> builder)
    {
        base.Configure(builder);
        builder.ToTable("payments");
        builder.Property(p => p.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(p => p.Amount).HasColumnName("amount").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.TransactionId).HasColumnName("transaction_id").IsRequired();
        builder.Property(p => p.PaymentDate).HasColumnName("payment_date").IsRequired();
        builder.Property(p => p.PaymentMethod).HasColumnName("payment_method").IsRequired();
        builder.Property(p => p.IsSuccessful).HasColumnName("is_successful").IsRequired();

        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId);
    }
}