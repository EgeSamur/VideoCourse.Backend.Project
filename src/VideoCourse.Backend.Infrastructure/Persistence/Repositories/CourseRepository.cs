using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;
// Course Repository Implementation
public class CourseRepository : RepositoryBase<Course, ApplicationDbContext>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context) { }
}
