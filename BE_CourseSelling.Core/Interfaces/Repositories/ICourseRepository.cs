using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE_CourseSelling.Core.Entities;

namespace BE_CourseSelling.Core.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<CourseDTO>
    {
        // Add course-specific data access methods here if needed
    }
}
