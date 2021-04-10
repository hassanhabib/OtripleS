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
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.ExamAttachments
{
    public partial class ExamAttachmentService
    {
        private delegate ValueTask<ExamAttachment> ReturningExamAttachmentFunction();
        private delegate IQueryable<ExamAttachment> ReturningExamAttachmentsFunction();

        private async ValueTask<ExamAttachment> TryCatch(
            ReturningExamAttachmentFunction returningExamAttachmentFunction)
        {
            try
            {
                return await returningExamAttachmentFunction();
            }

            catch (NullExamAttachmentException nullExamAttachmentException)
            {
                throw CreateAndLogValidationException(nullExamAttachmentException);
            }
            catch (InvalidExamAttachmentException invalidExamAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidExamAttachmentInputException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundExamAttachmentException notFoundExamAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundExamAttachmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsExamAttachmentException =
                    new AlreadyExistsExamAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsExamAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidExamAttachmentReferenceException =
                    new InvalidExamAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidExamAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedExamAttachmentException =
                    new LockedExamAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedExamAttachmentException);
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

        private IQueryable<ExamAttachment> TryCatch(
            ReturningExamAttachmentsFunction returningExamAttachmentsFunction)
        {
            try
            {
                return returningExamAttachmentsFunction();
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

        private ExamAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var ExamAttachmentValidationException = new ExamAttachmentValidationException(exception);
            this.loggingBroker.LogError(ExamAttachmentValidationException);

            return ExamAttachmentValidationException;
        }

        private ExamAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var examAttachmentDependencyException = new ExamAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(examAttachmentDependencyException);

            return examAttachmentDependencyException;
        }

        private ExamAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var examAttachmentDependencyException = new ExamAttachmentDependencyException(exception);
            this.loggingBroker.LogError(examAttachmentDependencyException);

            return examAttachmentDependencyException;
        }

        private ExamAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var ExamAttachmentServiceException = new ExamAttachmentServiceException(exception);
            this.loggingBroker.LogError(ExamAttachmentServiceException);

            return ExamAttachmentServiceException;
        }
    }
}
