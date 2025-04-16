using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// Video - Video entity'si
public class Video : BaseEntity
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string VideoUrl { get; set; } = String.Empty;
    public int DurationInSeconds { get; set; }
    public string ThumbnailUrl { get; set; } = String.Empty;

    // Navigation properties
    public ICollection<CourseSectionVideo> CourseSectionVideos { get; set; } 
    public ICollection<VideoProgress> UserProgresses { get; set; }
}
