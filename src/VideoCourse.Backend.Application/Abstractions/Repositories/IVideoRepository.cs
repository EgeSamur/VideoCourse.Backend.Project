using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// Video repository interface
public interface IVideoRepository : IReadRepository<Video>, IWriteRepository<Video> { }
