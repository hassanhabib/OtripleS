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
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentService
    {
        private delegate ValueTask<TeacherAttachment> ReturningTeacherAttachmentFunction();
        private delegate IQueryable<TeacherAttachment> ReturningTeacherAttachmentsFunction();

        private async ValueTask<TeacherAttachment> TryCatch(
            ReturningTeacherAttachmentFunction returningTeacherAttachmentFunction)
        {
            try
            {
                return await returningTeacherAttachmentFunction();
            }
            catch (NullTeacherAttachmentException nullTeacherAttachmentInputException)
            {
                throw CreateAndLogValidationException(nullTeacherAttachmentInputException);
            }
            catch (InvalidTeacherAttachmentException invalidTeacherAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidTeacherAttachmentInputException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundTeacherAttachmentException notFoundTeacherAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundTeacherAttachmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTeacherAttachmentException =
                    new AlreadyExistsTeacherAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsTeacherAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTeacherAttachmentReferenceException =
                    new InvalidTeacherAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidTeacherAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeacherAttachmentException =
                    new LockedTeacherAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedTeacherAttachmentException);
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

        private IQueryable<TeacherAttachment> TryCatch(
            ReturningTeacherAttachmentsFunction returningTeacherAttachmentsFunction)
        {
            try
            {
                return returningTeacherAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private TeacherAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var teacherAttachmentValidationException = new TeacherAttachmentValidationException(exception);
            this.loggingBroker.LogError(teacherAttachmentValidationException);

            return teacherAttachmentValidationException;
        }

        private TeacherAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var teacherAttachmentDependencyException = new TeacherAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(teacherAttachmentDependencyException);

            return teacherAttachmentDependencyException;
        }

        private TeacherAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var teacherAttachmentDependencyException = new TeacherAttachmentDependencyException(exception);
            this.loggingBroker.LogError(teacherAttachmentDependencyException);

            return teacherAttachmentDependencyException;
        }

        private TeacherAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var teacherAttachmentServiceException = new TeacherAttachmentServiceException(exception);
            this.loggingBroker.LogError(teacherAttachmentServiceException);

            return teacherAttachmentServiceException;
        }
    }
}
