using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// SectionVideo repository interface
public interface ICourseSectionVideoRepository : IReadRepository<CourseSectionVideo>, IWriteRepository<CourseSectionVideo> { }