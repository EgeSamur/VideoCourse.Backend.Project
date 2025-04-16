using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;

public class CourseCourseSectionRepository : RepositoryBase<CourseCourseSection, ApplicationDbContext>, ICourseCourseSectionRepository
{
    public CourseCourseSectionRepository(ApplicationDbContext context) : base(context) { }
}