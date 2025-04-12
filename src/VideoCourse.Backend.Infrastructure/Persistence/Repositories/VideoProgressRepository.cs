using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;
// VideoProgress Repository Implementation
public class VideoProgressRepository : RepositoryBase<VideoProgress, ApplicationDbContext>, IVideoProgressRepository
{
    public VideoProgressRepository(ApplicationDbContext context) : base(context) { }
}
