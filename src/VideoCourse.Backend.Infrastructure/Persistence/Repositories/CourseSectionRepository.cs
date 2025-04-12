using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;

namespace VideoCourse.Backend.Infrastructure.Persistence.Repositories
{
    public class CourseSectionRepository : RepositoryBase<CourseSection, ApplicationDbContext>, ICourseSectionRepository
    {
        public CourseSectionRepository(ApplicationDbContext context) : base(context) { }
    }
}
