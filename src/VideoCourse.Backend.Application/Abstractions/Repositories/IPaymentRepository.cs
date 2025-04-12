using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
namespace VideoCourse.Backend.Application.Abstractions.Repositories;

// Payment repository interface
public interface IPaymentRepository : IReadRepository<Payment>, IWriteRepository<Payment> { }