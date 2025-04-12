using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

public interface ICourseSectionRepository : IReadRepository<CourseSection>, IWriteRepository<CourseSection> { }
