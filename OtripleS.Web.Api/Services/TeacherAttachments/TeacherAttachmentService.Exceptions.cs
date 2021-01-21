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

namespace OtripleS.Web.Api.Services.TeacherAttachments
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

        private TeacherAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var TeacherAttachmentValidationException = new TeacherAttachmentValidationException(exception);
            this.loggingBroker.LogError(TeacherAttachmentValidationException);

            return TeacherAttachmentValidationException;
        }

        private TeacherAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var TeacherAttachmentDependencyException = new TeacherAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(TeacherAttachmentDependencyException);

            return TeacherAttachmentDependencyException;
        }

        private TeacherAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var TeacherAttachmentDependencyException = new TeacherAttachmentDependencyException(exception);
            this.loggingBroker.LogError(TeacherAttachmentDependencyException);

            return TeacherAttachmentDependencyException;
        }

        private TeacherAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var TeacherAttachmentServiceException = new TeacherAttachmentServiceException(exception);
            this.loggingBroker.LogError(TeacherAttachmentServiceException);

            return TeacherAttachmentServiceException;
        }
    }
}
