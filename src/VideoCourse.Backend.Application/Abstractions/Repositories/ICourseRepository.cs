using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// Course repository interface
public interface ICourseRepository : IReadRepository<Course>, IWriteRepository<Course> { }
