// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;

namespace OtripleS.Web.Api.Services.CalendarEntries
{
    public partial class CalendarEntryService
    {
        private void ValidateCalendarEntryOnCreate(CalendarEntry calendarEntry)
        {
            ValidateCalendarEntryIsNotNull(calendarEntry);
            ValidateCalendarEntryId(calendarEntry.Id);
            ValidateCalendarEntryRequiredFields(calendarEntry);
            ValidateCalendarEntryAuditFieldsOnCreate(calendarEntry);
        }
        
        private void ValidateCalendarEntryOnModify(CalendarEntry calendarEntry)
        {
            ValidateCalendarEntryIsNotNull(calendarEntry);
            ValidateCalendarEntryId(calendarEntry.Id);
        }

        private static void ValidateCalendarEntryIsNotNull(CalendarEntry CalendarEntry)
        {
            if (CalendarEntry is null)
            {
                throw new NullCalendarEntryException();
            }
        }

        private static void ValidateCalendarEntryId(Guid calendarEntryId)
        {
            if (IsInvalid(calendarEntryId))
            {
                throw new InvalidCalendarEntryException(
                    parameterName: nameof(CalendarEntry.Id),
                    parameterValue: calendarEntryId);
            }
        }

        private static void ValidateStorageCalendarEntry(
            CalendarEntry storageCalendarEntry,
            Guid calendarEntryId)
        {
            if (storageCalendarEntry == null)
            {
                throw new NotFoundCalendarEntryException(calendarEntryId);
            }
        }

        private static bool IsInvalid(Guid input) => input == Guid.Empty;
        private bool IsInvalid(DateTimeOffset input) => input == default;
        private bool IsInvalid(string input) => string.IsNullOrWhiteSpace(input);

        private void ValidateCalendarEntryAuditFieldsOnCreate(CalendarEntry calendarEntry)
        {
            switch (calendarEntry)
            {
                case { } when IsInvalid(input: calendarEntry.CreatedBy):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.CreatedBy),
                        parameterValue: calendarEntry.CreatedBy);

                case { } when IsInvalid(input: calendarEntry.CreatedDate):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.CreatedDate),
                        parameterValue: calendarEntry.CreatedDate);

                case { } when IsInvalid(input: calendarEntry.UpdatedBy):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.UpdatedBy),
                        parameterValue: calendarEntry.UpdatedBy);

                case { } when IsInvalid(input: calendarEntry.UpdatedDate):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.UpdatedDate),
                        parameterValue: calendarEntry.UpdatedDate);

                case { } when calendarEntry.UpdatedBy != calendarEntry.CreatedBy:
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.UpdatedBy),
                        parameterValue: calendarEntry.UpdatedBy);

                case { } when calendarEntry.UpdatedDate != calendarEntry.CreatedDate:
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.UpdatedDate),
                        parameterValue: calendarEntry.UpdatedDate);

                case { } when IsDateNotRecent(calendarEntry.CreatedDate):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.CreatedDate),
                        parameterValue: calendarEntry.CreatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateCalendarEntryRequiredFields(CalendarEntry calendarEntry)
        {
            switch (calendarEntry)
            {
                case { } when IsInvalid(input: calendarEntry.Label):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.Label),
                        parameterValue: calendarEntry.Label);

                case { } when IsInvalid(input: calendarEntry.Description):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.Description),
                        parameterValue: calendarEntry.Description);
            }
        }

        private void ValidateStorageCalendarEntries(IQueryable<CalendarEntry> storageCalendarEntries)
        {
            if (storageCalendarEntries.Count() == 0)
            {
                this.loggingBroker.LogWarning("No calendar entries found in storage.");
            }
        }
    }
}
