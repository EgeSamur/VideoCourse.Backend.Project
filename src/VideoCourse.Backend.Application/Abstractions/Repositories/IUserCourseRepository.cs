using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// UserCourse repository interface
public interface IUserCourseRepository : IReadRepository<UserCourse>, IWriteRepository<UserCourse> { }
