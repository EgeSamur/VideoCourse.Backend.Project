using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.ToTable("users");
        builder.Property(u => u.FullName).HasColumnName("full_name").IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").IsRequired();
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired();
        builder.Property(u => u.PasswordSalt).HasColumnName("password_salt").IsRequired();
        builder.Property(u => u.PhoneNumber).HasColumnName("phone_number");
        builder.Property(u => u.Job).HasColumnName("job");
        builder.Property(u => u.ProfileImageUrl).HasColumnName("profile_image_url");
        builder.Property(u => u.IsAdmin).HasColumnName("is_admin").IsRequired();
        builder.Property(u => u.LastLoginDate).HasColumnName("last_login_date");

        // Indexes
        builder.HasIndex(u => u.Email).IsUnique();
    }
}
