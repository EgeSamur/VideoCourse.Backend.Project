using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// VideoProgress repository interface
public interface IVideoProgressRepository : IReadRepository<VideoProgress>, IWriteRepository<VideoProgress> { }
