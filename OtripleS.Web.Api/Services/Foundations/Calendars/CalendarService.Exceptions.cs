﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Calendars
{
    public partial class CalendarService
    {
        private delegate ValueTask<Calendar> ReturningCalendarFunction();
        private delegate IQueryable<Calendar> ReturningCalendarsFunction();

        private async ValueTask<Calendar> TryCatch(ReturningCalendarFunction returningCalendarFunction)
        {
            try
            {
                return await returningCalendarFunction();
            }
            catch (NullCalendarException nullCalendarException)
            {
                throw CreateAndLogValidationException(nullCalendarException);
            }
            catch (InvalidCalendarException invalidCalendarException)
            {
                throw CreateAndLogValidationException(invalidCalendarException);
            }
            catch (NotFoundCalendarException nullCalendarException)
            {
                throw CreateAndLogValidationException(nullCalendarException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCalendarException =
                    new AlreadyExistsCalendarException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCalendarException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCalendarException = new LockedCalendarException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedCalendarException);
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

        private IQueryable<Calendar> TryCatch(ReturningCalendarsFunction returningCalendarsFunction)
        {
            try
            {
                return returningCalendarsFunction();
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

        private CalendarValidationException CreateAndLogValidationException(Exception exception)
        {
            var calendarValidationException = new CalendarValidationException(exception);
            this.loggingBroker.LogError(calendarValidationException);

            return calendarValidationException;
        }

        private CalendarDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var calendarDependencyException = new CalendarDependencyException(exception);
            this.loggingBroker.LogCritical(calendarDependencyException);

            return calendarDependencyException;
        }

        private CalendarDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var calendarDependencyException = new CalendarDependencyException(exception);
            this.loggingBroker.LogError(calendarDependencyException);

            return calendarDependencyException;
        }

        private CalendarServiceException CreateAndLogServiceException(Exception exception)
        {
            var calendarServiceException = new CalendarServiceException(exception);
            this.loggingBroker.LogError(calendarServiceException);

            return calendarServiceException;
        }
    }
}
