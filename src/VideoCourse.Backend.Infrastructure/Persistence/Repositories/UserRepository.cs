using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;

public class UserRepository : RepositoryBase<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }
}