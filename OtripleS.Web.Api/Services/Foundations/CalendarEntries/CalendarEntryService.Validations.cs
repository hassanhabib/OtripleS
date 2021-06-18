// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Foundations.CalendarEntries;
using OtripleS.Web.Api.Models.Foundations.CalendarEntries.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntries
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
            ValidateCalendarEntryRequiredFields(calendarEntry);
            ValidateCalendarEntryAuditFieldsOnModify(calendarEntry);
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
        private static bool IsInvalid(DateTimeOffset input) => input == default;
        private static bool IsInvalid(string input) => string.IsNullOrWhiteSpace(input);

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

        private void ValidateCalendarEntryAuditFieldsOnModify(CalendarEntry calendarEntry)
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

                case { } when calendarEntry.UpdatedDate == calendarEntry.CreatedDate:
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.UpdatedDate),
                        parameterValue: calendarEntry.UpdatedDate);

                case { } when IsDateNotRecent(calendarEntry.UpdatedDate):
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.UpdatedDate),
                        parameterValue: calendarEntry.UpdatedDate);
            }
        }

        private static void ValidateAgainstStorageCalendarEntryOnModify(
            CalendarEntry inputCalendarEntry,
            CalendarEntry storageCalendarEntry)
        {
            switch (inputCalendarEntry)
            {
                case { } when inputCalendarEntry.CreatedDate != storageCalendarEntry.CreatedDate:
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.CreatedDate),
                        parameterValue: inputCalendarEntry.CreatedDate);

                case { } when inputCalendarEntry.CreatedBy != storageCalendarEntry.CreatedBy:
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.CreatedBy),
                        parameterValue: inputCalendarEntry.CreatedBy);

                case { } when inputCalendarEntry.UpdatedDate == storageCalendarEntry.UpdatedDate:
                    throw new InvalidCalendarEntryException(
                        parameterName: nameof(CalendarEntry.UpdatedDate),
                        parameterValue: inputCalendarEntry.UpdatedDate);
            }
        }


        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateCalendarEntryRequiredFields(CalendarEntry calendarEntry)
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
            if (!storageCalendarEntries.Any())
            {
                this.loggingBroker.LogWarning("No calendar entries found in storage.");
            }
        }
    }
}
