using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// UserCourse - Kullanıcı ve kurs arasındaki many-to-many ilişkisi (kayıt)
public class UserCourse : BaseEntity
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;

    // Navigation properties
    public User User { get; set; } = new User();
    public Course Course { get; set; } = new Course();
}
