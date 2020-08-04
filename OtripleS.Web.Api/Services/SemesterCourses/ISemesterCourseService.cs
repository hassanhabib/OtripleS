using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.SemesterCourses;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public interface ISemesterCourseService
    {
        ValueTask<SemesterCourse> DeleteSemesterCourseAsync(Guid semesterCourseId);
    }
}