// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;

namespace OtripleS.Web.Api.Services.Courses
{
    public partial class CourseService
    {
        private void ValidateCourseId(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                throw new InvalidCourseInputException(
                    parameterName: nameof(Course.Id),
                    parameterValue: courseId);
            }
        }

        private static void ValidateStorageCourse(Course storageCourse, Guid courseId)
        {
            if (storageCourse == null)
            {
                throw new NotFoundCourseException(courseId);
            }
        }

        private void ValidateStorageCourses(IQueryable<Course> storageCourses)
        {
            if (storageCourses.Count() == 0)
            {
                this.loggingBroker.LogWarning("No courses found in storage.");
            }
        }
    }
}
