// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;

namespace OtripleS.Web.Api.Services.Courses
{
    public partial class CourseService
    {
        private delegate ValueTask<Course> ReturningCourseFunction();

        private async ValueTask<Course> TryCatch(ReturningCourseFunction returningCourseFunction)
        {
            try
            {
                return await returningCourseFunction();
            }
            catch (InvalidCourseInputException invalidCourseInputException)
            {
                throw CreateAndLogValidationException(invalidCourseInputException);
            }
            catch (NotFoundCourseException notFoundCourseException)
            {
                throw CreateAndLogValidationException(notFoundCourseException);
            }
        }

        private CourseValidationException CreateAndLogValidationException(Exception exception)
        {
            var courseValidationException = new CourseValidationException(exception);
            this.loggingBroker.LogError(courseValidationException);

            return courseValidationException;
        }
    }
}
