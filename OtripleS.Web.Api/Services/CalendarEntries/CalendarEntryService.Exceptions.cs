// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;

namespace OtripleS.Web.Api.Services.CalendarEntries
{
    public partial class CalendarEntryService
    {
        private delegate IQueryable<CalendarEntry> ReturningCalendarEntriesFunction();

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
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private CalendarEntryDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var calendarEntryDependencyException = new CalendarEntryDependencyException(exception);
            this.loggingBroker.LogCritical(calendarEntryDependencyException);

            return calendarEntryDependencyException;
        }

        private CalendarEntryDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var calendarEntryDependencyException = new CalendarEntryDependencyException(exception);
            this.loggingBroker.LogError(calendarEntryDependencyException);

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
