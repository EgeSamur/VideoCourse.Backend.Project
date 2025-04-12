namespace VideoCourse.Backend.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    ICourseRepository CourseRepository { get; }
    ICourseSectionRepository CourseSectionRepository { get; }
    ISectionVideoRepository SectionVideoRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IUserCourseRepository UserCourseRepository { get; }
    IVideoRepository VideoRepository { get; }
    IVideoProgressRepository VideoProgressRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void Rollback();
}