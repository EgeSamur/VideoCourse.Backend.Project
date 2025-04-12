using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;
// Payment Repository Implementation
public class PaymentRepository : RepositoryBase<Payment, ApplicationDbContext>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context) { }
}
