using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<VideoProgress> VideoProgresses { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseSection> CourseSections { get; set; }
    public DbSet<SectionVideo> SectionVideos { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}