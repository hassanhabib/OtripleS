using OtripleS.Web.Api.Models.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Courses
{
    public interface ICourseService
    {
        ValueTask<Course> ModifyCourseAsync(Course course);
    }
}
