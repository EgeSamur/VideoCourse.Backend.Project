using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// User repository interface
public interface IUserRepository : IReadRepository<User>, IWriteRepository<User> { }
