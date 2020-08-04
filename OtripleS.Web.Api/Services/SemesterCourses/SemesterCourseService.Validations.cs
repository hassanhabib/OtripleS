using System;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public partial class SemesterCourseService
    {
        private void ValidateSemesterCourseServiceIdIsNull(Guid semesterCourseId)
        {
            if (semesterCourseId == default)
            {
                throw new InvalidSemesterCourseInputException(
                    parameterName: nameof(SemesterCourse.Id),
                    parameterValue: semesterCourseId);
            }
        }
    }
}