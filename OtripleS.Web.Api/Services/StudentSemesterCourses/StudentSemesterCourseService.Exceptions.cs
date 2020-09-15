//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.StudentSemesterCourses
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
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
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
                throw CreateAndLogServiceException(exception);
            }
        }

        private StudentSemesterCourseServiceException CreateAndLogServiceException(Exception exception)
        {
            var StudentSemesterCourseServiceException = new StudentSemesterCourseServiceException(exception);
            this.loggingBroker.LogError(StudentSemesterCourseServiceException);

            return StudentSemesterCourseServiceException;
        }

        private StudentSemesterCourseValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentSemesterCourseValidationException = new StudentSemesterCourseValidationException(exception);
            this.loggingBroker.LogError(StudentSemesterCourseValidationException);

            return StudentSemesterCourseValidationException;
        }

        private StudentSemesterCourseDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentSemesterCourseDependencyException = new StudentSemesterCourseDependencyException(exception);
            this.loggingBroker.LogCritical(StudentSemesterCourseDependencyException);

            return StudentSemesterCourseDependencyException;
        }

        private StudentSemesterCourseDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var StudentSemesterCourseDependencyException = new StudentSemesterCourseDependencyException(exception);
            this.loggingBroker.LogError(StudentSemesterCourseDependencyException);

            return StudentSemesterCourseDependencyException;
        }
    }
}
