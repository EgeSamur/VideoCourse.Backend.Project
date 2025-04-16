using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// User - Kullanıcı entity'si
public class User : BaseEntity
{
    public string FullName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];
    public string? PhoneNumber { get; set; } = null;
    public string? Job { get; set; } = null;
    public string? ProfileImageUrl { get; set; } = null;
    public bool IsAdmin { get; set; } = false;
    public DateTime? LastLoginDate { get; set; }

    // Navigation properties
    public ICollection<UserCourse> EnrolledCourses { get; set; } 
    public ICollection<VideoProgress> VideoProgresses { get; set; } 
    public ICollection<Payment> Payments { get; set; } 
}
