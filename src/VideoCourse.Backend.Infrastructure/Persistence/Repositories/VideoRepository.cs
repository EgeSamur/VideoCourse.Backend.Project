using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;
// Video Repository Implementation
public class VideoRepository : RepositoryBase<Video, ApplicationDbContext>, IVideoRepository
{
    public VideoRepository(ApplicationDbContext context) : base(context) { }
}
