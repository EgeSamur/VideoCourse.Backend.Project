using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;
// UserCourse Repository Implementation
public class UserCourseRepository : RepositoryBase<UserCourse, ApplicationDbContext>, IUserCourseRepository
{
    public UserCourseRepository(ApplicationDbContext context) : base(context) { }
}
