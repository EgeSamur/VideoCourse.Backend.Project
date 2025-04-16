using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Shared.Persistence.EfCore;

namespace VideoCourse.Backend.Infrastructure.Persistence.Repositories
{

    // SectionVideo Repository Implementation
    public class CourseSectionVideoRepository : RepositoryBase<CourseSectionVideo, ApplicationDbContext>, ICourseSectionVideoRepository
    {
        public CourseSectionVideoRepository(ApplicationDbContext context) : base(context) { }
    }
}
