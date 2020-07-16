using OtripleS.Web.Api.Models.Courses;

using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Courses
{
    public interface ICourseService
    {
        ValueTask<Course> CreateCourseAsync(Course course);
    }
}
