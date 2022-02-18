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
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntries
{
    public partial class CalendarEntryService
    {
        private delegate ValueTask<CalendarEntry> ReturningCalendarEntryFunction();
        private delegate IQueryable<CalendarEntry> ReturningCalendarEntriesFunction();

        private async ValueTask<CalendarEntry> TryCatch(ReturningCalendarEntryFunction returningCalendarEntryFunction)
        {
            try
            {
                return await returningCalendarEntryFunction();
            }
            catch (NullCalendarEntryException nullCalendarEntryException)
            {
                throw CreateAndLogValidationException(nullCalendarEntryException);
            }
            catch (InvalidCalendarEntryException invalidCalendarEntryException)
            {
                throw CreateAndLogValidationException(invalidCalendarEntryException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundCalendarEntryException notFoundCalendarEntryException)
            {
                throw CreateAndLogValidationException(notFoundCalendarEntryException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCalendarEntryException =
                new AlreadyExistsCalendarEntryException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCalendarEntryException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCalendarException =
                    new LockedCalendarEntryException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCalendarException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedCalendarEntryServiceException =
                    new FailedCalendarEntryServiceException(exception);

                throw CreateAndLogServiceException(failedCalendarEntryServiceException);
            }
        }

        private IQueryable<CalendarEntry> TryCatch(ReturningCalendarEntriesFunction returningCalendarEntriesFunction)
        {
            try
            {
                return returningCalendarEntriesFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedCalendarEntryServiceException =
                new FailedCalendarEntryServiceException(exception);

                throw CreateAndLogServiceException(failedCalendarEntryServiceException);
            }
        }

        private CalendarEntryValidationException CreateAndLogValidationException(Exception exception)
        {
            var calendarEntryValidationException = new CalendarEntryValidationException(exception);
            this.loggingBroker.LogError(calendarEntryValidationException);

            return calendarEntryValidationException;
        }

        private CalendarEntryDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var calendarEntryDependencyException = new CalendarEntryDependencyException(exception);
            this.loggingBroker.LogError(calendarEntryDependencyException);

            return calendarEntryDependencyException;
        }

        private CalendarEntryDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var calendarEntryDependencyException = new CalendarEntryDependencyException(exception);
            this.loggingBroker.LogCritical(calendarEntryDependencyException);

            return calendarEntryDependencyException;
        }

        private CalendarEntryServiceException CreateAndLogServiceException(Exception exception)
        {
            var calendarEntryServiceException = new CalendarEntryServiceException(exception);
            this.loggingBroker.LogError(calendarEntryServiceException);

            return calendarEntryServiceException;
        }
    }
}
