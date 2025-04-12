using Microsoft.EntityFrameworkCore;
using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;

namespace VideoCourse.Backend.Infrastructure.Persistence.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, ICourseRepository courseRepository, IPaymentRepository paymentRepository, IUserCourseRepository userCourseRepository, IVideoProgressRepository videoProgressRepository, IVideoRepository videoRepository, ICourseSectionRepository courseSectionRepository, ISectionVideoRepository sectionVideoRepository)
    {
        _context = context;
        UserRepository = userRepository;
        CourseRepository = courseRepository;
        PaymentRepository = paymentRepository;
        UserCourseRepository = userCourseRepository;
        VideoProgressRepository = videoProgressRepository;
        VideoRepository = videoRepository;
        CourseSectionRepository = courseSectionRepository;
        SectionVideoRepository = sectionVideoRepository;
    }

    private readonly ApplicationDbContext _context;

    public IUserRepository UserRepository { get; }
    public ICourseRepository CourseRepository { get; }
    public ICourseSectionRepository CourseSectionRepository { get; }
    public ISectionVideoRepository SectionVideoRepository { get; }
    public IPaymentRepository PaymentRepository { get; }
    public IUserCourseRepository UserCourseRepository { get; }
    public IVideoProgressRepository VideoProgressRepository { get; }
    public IVideoRepository VideoRepository { get; }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Rollback()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }
}
