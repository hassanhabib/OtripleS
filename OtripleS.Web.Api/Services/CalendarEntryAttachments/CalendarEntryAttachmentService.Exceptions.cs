// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------


using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private delegate ValueTask<CalendarEntryAttachment> ReturningCalendarEntryAttachmentFunction();
        private delegate IQueryable<CalendarEntryAttachment> ReturningCalendarEntryAttachmentsFunction();

        private async ValueTask<CalendarEntryAttachment> TryCatch(
            ReturningCalendarEntryAttachmentFunction returningCalendarEntryAttachmentFunction)
        {
            try
            {
                return await returningCalendarEntryAttachmentFunction();
            }
            catch (NullCalendarEntryAttachmentException nullCalendarEntryAttachmentException)
            {
                throw CreateAndLogValidationException(nullCalendarEntryAttachmentException);
            }
            catch (InvalidCalendarEntryAttachmentException invalidCalendarEntryAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidCalendarEntryAttachmentInputException);
            }
            catch (NotFoundCalendarEntryAttachmentException notFoundCalendarEntryAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundCalendarEntryAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCalendarEntryAttachmentException =
                    new AlreadyExistsCalendarEntryAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCalendarEntryAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidCalendarEntryAttachmentReferenceException =
                    new InvalidCalendarEntryAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidCalendarEntryAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCalendarEntryAttachmentException =
                    new LockedCalendarEntryAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCalendarEntryAttachmentException);
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

        private IQueryable<CalendarEntryAttachment> TryCatch(ReturningCalendarEntryAttachmentsFunction returningCalendarEntryAttachmentsFunction)
        {
            try
            {
                return returningCalendarEntryAttachmentsFunction();
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

        private CalendarEntryAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var CalendarEntryAttachmentValidationException = new CalendarEntryAttachmentValidationException(exception);
            this.loggingBroker.LogError(CalendarEntryAttachmentValidationException);

            return CalendarEntryAttachmentValidationException;
        }

        private CalendarEntryAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var CalendarEntryAttachmentDependencyException = new CalendarEntryAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(CalendarEntryAttachmentDependencyException);

            return CalendarEntryAttachmentDependencyException;
        }

        private CalendarEntryAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var CalendarEntryAttachmentDependencyException = new CalendarEntryAttachmentDependencyException(exception);
            this.loggingBroker.LogError(CalendarEntryAttachmentDependencyException);

            return CalendarEntryAttachmentDependencyException;
        }

        private CalendarEntryAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var CalendarEntryAttachmentServiceException = new CalendarEntryAttachmentServiceException(exception);
            this.loggingBroker.LogError(CalendarEntryAttachmentServiceException);

            return CalendarEntryAttachmentServiceException;
        }
    }
}
