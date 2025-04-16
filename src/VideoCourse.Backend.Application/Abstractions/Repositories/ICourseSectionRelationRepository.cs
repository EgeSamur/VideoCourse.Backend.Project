using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

public interface ICourseCourseSectionRepository : IReadRepository<CourseCourseSection>, IWriteRepository<CourseCourseSection> { }