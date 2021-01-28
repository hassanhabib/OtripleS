//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private delegate ValueTask<CalendarEntryAttachment> ReturningCalendarEntryAttachmentFunction();

        private async ValueTask<CalendarEntryAttachment> TryCatch(
            ReturningCalendarEntryAttachmentFunction returningCalendarEntryAttachmentFunction)
        {
            try
            {
                return await returningCalendarEntryAttachmentFunction();
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

        private CalendarEntryAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var calendarEntryAttachmentValidationException = new CalendarEntryAttachmentValidationException(exception);
            this.loggingBroker.LogError(calendarEntryAttachmentValidationException);

            return calendarEntryAttachmentValidationException;
        }

        private CalendarEntryAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var calendarEntryAttachmentDependencyException = new CalendarEntryAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(calendarEntryAttachmentDependencyException);

            return calendarEntryAttachmentDependencyException;
        }

        private CalendarEntryAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var calendarEntryAttachmentDependencyException = new CalendarEntryAttachmentDependencyException(exception);
            this.loggingBroker.LogError(calendarEntryAttachmentDependencyException);

            return calendarEntryAttachmentDependencyException;
        }

        private CalendarEntryAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var CalendarEntryAttachmentServiceException = new CalendarEntryAttachmentServiceException(exception);
            this.loggingBroker.LogError(CalendarEntryAttachmentServiceException);

            return CalendarEntryAttachmentServiceException;
        }
    }
}
