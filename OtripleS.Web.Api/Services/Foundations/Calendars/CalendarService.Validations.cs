// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Foundations.Calendars;
using OtripleS.Web.Api.Models.Foundations.Calendars.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Calendars
{
    public partial class CalendarService
    {
        private void ValidateCalendarOnCreate(Calendar calendar)
        {
            ValidateCalendarIsNull(calendar);
            ValidateCalendarIdIsNull(calendar.Id);
            ValidateCalendarFields(calendar);
            ValidateInvalidAuditFields(calendar);
            ValidateAuditFieldDataAreSame(calendar);
            ValidateCreatedDateIsRecent(calendar);
        }

        private void ValidateCalendarOnModify(Calendar calendar)
        {
            ValidateCalendarIsNull(calendar);
            ValidateCalendarIdIsNull(calendar.Id);
            ValidateCalendarFields(calendar);
            ValidateInvalidAuditFields(calendar);
            ValidateDatesAreNotSame(calendar);
            ValidateUpdatedDateIsRecent(calendar);
        }

        private void ValidateStorageCalendars(IQueryable<Calendar> storageCalendar)
        {
            if (!storageCalendar.Any())
            {
                this.loggingBroker.LogWarning("No calendars found in storage.");
            }
        }

        private static void ValidateCalendarIsNull(Calendar calendar)
        {
            if (calendar is null)
            {
                throw new NullCalendarException();
            }
        }

        private static void ValidateAgainstStorageCalendarOnModify(Calendar inputCalendar, Calendar storageCalendar)
        {
            switch (inputCalendar)
            {
                case { } when inputCalendar.CreatedDate != storageCalendar.CreatedDate:
                    throw new InvalidCalendarInputException(
                        parameterName: nameof(Calendar.CreatedDate),
                        parameterValue: inputCalendar.CreatedDate);

                case { } when inputCalendar.CreatedBy != storageCalendar.CreatedBy:
                    throw new InvalidCalendarInputException(
                        parameterName: nameof(Calendar.CreatedBy),
                        parameterValue: inputCalendar.CreatedBy);

                case { } when inputCalendar.UpdatedDate == storageCalendar.UpdatedDate:
                    throw new InvalidCalendarInputException(
                        parameterName: nameof(Calendar.UpdatedDate),
                        parameterValue: inputCalendar.UpdatedDate);
            }
        }

        private static void ValidateCalendarIdIsNull(Guid calendarId)
        {
            if (IsInvalid(calendarId))
            {
                throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.Id),
                    parameterValue: calendarId);
            }
        }

        private static void ValidateCalendarFields(Calendar calendar)
        {
            if (IsInvalid(calendar.Label))
            {
                throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.Label),
                    parameterValue: calendar.Label);
            }
        }

        private static void ValidateInvalidAuditFields(Calendar calendar)
        {
            switch (calendar)
            {
                case { } when IsInvalid(calendar.CreatedBy):
                    throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.CreatedBy),
                    parameterValue: calendar.CreatedBy);

                case { } when IsInvalid(calendar.CreatedDate):
                    throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.CreatedDate),
                    parameterValue: calendar.CreatedDate);

                case { } when IsInvalid(calendar.UpdatedBy):
                    throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.UpdatedBy),
                    parameterValue: calendar.UpdatedBy);

                case { } when IsInvalid(calendar.UpdatedDate):
                    throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.UpdatedDate),
                    parameterValue: calendar.UpdatedDate);
            }
        }

        private static void ValidateAuditFieldDataAreSame(Calendar calendar)
        {
            switch (calendar)
            {
                case { } when calendar.CreatedBy != calendar.UpdatedBy:
                    throw new InvalidCalendarInputException(
                        parameterName: nameof(Calendar.UpdatedBy),
                        parameterValue: calendar.UpdatedBy);

                case { } when calendar.CreatedDate != calendar.UpdatedDate:
                    throw new InvalidCalendarInputException(
                        parameterName: nameof(Calendar.UpdatedDate),
                        parameterValue: calendar.UpdatedDate);
            }
        }

        private static void ValidateDatesAreNotSame(Calendar calendar)
        {
            if (calendar.CreatedDate == calendar.UpdatedDate)
            {
                throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.UpdatedDate),
                    parameterValue: calendar.UpdatedDate);
            }
        }

        private void ValidateCreatedDateIsRecent(Calendar calendar)
        {
            if (IsDateNotRecent(calendar.CreatedDate))
            {
                throw new InvalidCalendarInputException(
                    parameterName: nameof(calendar.CreatedDate),
                    parameterValue: calendar.CreatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Calendar calendar)
        {
            if (IsDateNotRecent(calendar.UpdatedDate))
            {
                throw new InvalidCalendarInputException(
                    parameterName: nameof(calendar.UpdatedDate),
                    parameterValue: calendar.UpdatedDate);
            }
        }

        private static void ValidateCalendarId(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new InvalidCalendarInputException(
                    parameterName: nameof(Calendar.Id),
                    parameterValue: calendarId);
            }
        }

        private static void ValidateStorageCalendar(Calendar storageCalendar, Guid calendarId)
        {
            if (storageCalendar == null)
            {
                throw new NotFoundCalendarException(calendarId);
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
