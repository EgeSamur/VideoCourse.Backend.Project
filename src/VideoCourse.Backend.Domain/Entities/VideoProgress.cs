using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// VideoProgress - Kullanıcı video ilerleme durumu
public class VideoProgress : BaseEntity
{
    public int UserId { get; set; }
    public int VideoId { get; set; }
    public bool IsCompleted { get; set; } = false;
    public int LastWatchedPositionInSeconds { get; set; } = 0;
    public DateTime LastWatchedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = new User();
    public Video Video { get; set; } = new Video();
}
