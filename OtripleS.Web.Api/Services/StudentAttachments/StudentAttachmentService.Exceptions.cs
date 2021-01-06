//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.StudentAttachments
{
    public partial class StudentAttachmentService
    {
        private delegate ValueTask<StudentAttachment> ReturningStudentAttachmentFunction();

        private async ValueTask<StudentAttachment> TryCatch(
            ReturningStudentAttachmentFunction returningStudentAttachmentFunction)
        {
            try
            {
                return await returningStudentAttachmentFunction();
            }
            catch (InvalidStudentAttachmentException invalidStudentAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidStudentAttachmentInputException);
            }
            catch (NotFoundStudentAttachmentException notFoundStudentAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundStudentAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSemesterCourseException =
                    new LockedStudentAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedSemesterCourseException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
        }

        private StudentAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentAttachmentValidationException = new StudentAttachmentValidationException(exception);
            this.loggingBroker.LogError(StudentAttachmentValidationException);

            return StudentAttachmentValidationException;
        }

        private StudentAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentAttachmentDependencyException = new StudentAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(StudentAttachmentDependencyException);

            return StudentAttachmentDependencyException;
        }

        private StudentAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var StudentAttachmentDependencyException = new StudentAttachmentDependencyException(exception);
            this.loggingBroker.LogError(StudentAttachmentDependencyException);

            return StudentAttachmentDependencyException;
        }
    }
}
