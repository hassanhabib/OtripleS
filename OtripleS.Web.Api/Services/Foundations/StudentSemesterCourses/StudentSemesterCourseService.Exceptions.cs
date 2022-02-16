// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentSemesterCourses
{
    public partial class StudentSemesterCourseService
    {
        private delegate ValueTask<StudentSemesterCourse> ReturningStudentSemesterCourseFunction();
        private delegate IQueryable<StudentSemesterCourse> ReturningStudentSemesterCoursesFunction();

        private IQueryable<StudentSemesterCourse> TryCatch(
            ReturningStudentSemesterCoursesFunction returningStudentSemesterCoursesFunction)
        {
            try
            {
                return returningStudentSemesterCoursesFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedStudentSemesterCourseServiceException =
                    new FailedStudentSemesterCourseServiceException(exception);

                throw CreateAndLogServiceException(failedStudentSemesterCourseServiceException);
            }
        }

        private async ValueTask<StudentSemesterCourse> TryCatch(
            ReturningStudentSemesterCourseFunction returningStudentSemesterCourseFunction)
        {
            try
            {
                return await returningStudentSemesterCourseFunction();
            }
            catch (NullStudentSemesterCourseException nullStudentSemesterCourseException)
            {
                throw CreateAndLogValidationException(nullStudentSemesterCourseException);
            }
            catch (NotFoundStudentSemesterCourseException notFoundStudentSemesterCourseException)
            {
                throw CreateAndLogValidationException(notFoundStudentSemesterCourseException);
            }
            catch (InvalidStudentSemesterCourseInputException invalidStudentSemesterCourseInputException)
            {
                throw CreateAndLogValidationException(invalidStudentSemesterCourseInputException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentSemesterCourseException =
                    new AlreadyExistsStudentSemesterCourseException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsStudentSemesterCourseException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSemesterCourseException =
                    new LockedStudentSemesterCourseException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedSemesterCourseException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedStudentSemesterCourseServiceException =
                    new FailedStudentSemesterCourseServiceException(exception);

                throw CreateAndLogServiceException(failedStudentSemesterCourseServiceException);
            }
        }

        private StudentSemesterCourseValidationException CreateAndLogValidationException(Exception exception)
        {
            var studentSemesterCourseValidationException = new StudentSemesterCourseValidationException(exception);
            this.loggingBroker.LogError(studentSemesterCourseValidationException);

            return studentSemesterCourseValidationException;
        }


        private StudentSemesterCourseDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentSemesterCourseDependencyException = new StudentSemesterCourseDependencyException(exception);
            this.loggingBroker.LogCritical(studentSemesterCourseDependencyException);

            return studentSemesterCourseDependencyException;
        }

        private StudentSemesterCourseDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentSemesterCourseDependencyException = new StudentSemesterCourseDependencyException(exception);
            this.loggingBroker.LogError(studentSemesterCourseDependencyException);

            return studentSemesterCourseDependencyException;
        }

        private StudentSemesterCourseServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentSemesterCourseServiceException = new StudentSemesterCourseServiceException(exception);
            this.loggingBroker.LogError(studentSemesterCourseServiceException);

            return studentSemesterCourseServiceException;
        }
    }
}
