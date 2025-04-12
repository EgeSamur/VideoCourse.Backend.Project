using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// SectionVideo repository interface
public interface ISectionVideoRepository : IReadRepository<SectionVideo>, IWriteRepository<SectionVideo> { }