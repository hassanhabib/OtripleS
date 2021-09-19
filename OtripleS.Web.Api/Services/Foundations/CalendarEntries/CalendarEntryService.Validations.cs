// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntries
{
    public partial class CalendarEntryService
    {
        private void ValidateCalendarEntryOnCreate(CalendarEntry calendarEntry)
        {
            ValidateCalendarEntryIsNotNull(calendarEntry);

            Validate
            (
                (Rule: IsInvalidX(calendarEntry.Id), Parameter: nameof(CalendarEntry.Id)),
                (Rule: IsInvalidX(calendarEntry.Label), Parameter: nameof(CalendarEntry.Label)),
                (Rule: IsInvalidX(calendarEntry.Description), Parameter: nameof(CalendarEntry.Description)),
                (Rule: IsInvalidX(calendarEntry.StartDate), Parameter: nameof(CalendarEntry.StartDate)),
                (Rule: IsInvalidX(calendarEntry.EndDate), Parameter: nameof(CalendarEntry.EndDate)),
                (Rule: IsInvalidX(calendarEntry.RemindAtDateTime), Parameter: nameof(CalendarEntry.RemindAtDateTime)),
                (Rule: IsInvalidX(calendarEntry.CreatedBy), Parameter: nameof(CalendarEntry.CreatedBy)),
                (Rule: IsInvalidX(calendarEntry.UpdatedBy), Parameter: nameof(CalendarEntry.UpdatedBy)),
                (Rule: IsInvalidX(calendarEntry.CreatedDate), Parameter: nameof(CalendarEntry.CreatedDate)),
                (Rule: IsInvalidX(calendarEntry.UpdatedDate), Parameter: nameof(CalendarEntry.UpdatedDate)),
                (Rule: IsNotRecent(calendarEntry.CreatedDate), Parameter: nameof(CalendarEntry.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: calendarEntry.UpdatedBy,
                    secondId: calendarEntry.CreatedBy,
                    secondIdName: nameof(CalendarEntry.CreatedBy)),
                Parameter: nameof(CalendarEntry.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: calendarEntry.UpdatedDate,
                    secondDate: calendarEntry.CreatedDate,
                    secondDateName: nameof(CalendarEntry.CreatedDate)),
                Parameter: nameof(CalendarEntry.UpdatedDate))
            );

        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCalendarEntryException = new InvalidCalendarEntryException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCalendarEntryException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCalendarEntryException.ThrowIfContainsErrors();
        }

        private static dynamic IsInvalidX(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidX(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

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
