// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;

namespace OtripleS.Web.Api.Services.CalendarEntries
{
    public partial class CalendarEntryService
    {
        private delegate ValueTask<CalendarEntry> ReturningCalendarEntryFunction();

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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCalendarEntryException =
                new AlreadyExistsCalendarEntryException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCalendarEntryException);
            }
        }

        private CalendarEntryValidationException CreateAndLogValidationException(Exception exception)
        {
            var calendarEntryValidationException = new CalendarEntryValidationException(exception);
            this.loggingBroker.LogError(calendarEntryValidationException);

            return calendarEntryValidationException;
        }

        private CalendarEntryDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var calendarEntryDependencyException = new CalendarEntryDependencyException(exception);
            this.loggingBroker.LogCritical(calendarEntryDependencyException);

            return calendarEntryDependencyException;
        }
    }
}
