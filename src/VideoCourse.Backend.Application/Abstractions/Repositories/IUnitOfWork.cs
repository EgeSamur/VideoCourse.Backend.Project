namespace VideoCourse.Backend.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    ICourseRepository CourseRepository { get; }
    ICourseSectionRepository CourseSectionRepository { get; }
    ICourseSectionVideoRepository ICourseSectionVideoRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IUserCourseRepository UserCourseRepository { get; }
    IVideoRepository VideoRepository { get; }
    IVideoProgressRepository VideoProgressRepository { get; }
    ICourseCourseSectionRepository CourseCourseSectionRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void Rollback();
}