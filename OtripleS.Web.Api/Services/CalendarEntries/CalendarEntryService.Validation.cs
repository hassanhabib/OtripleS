// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
            ValidateCalendarEntryAuditFieldsOnCreate(calendarEntry);
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

        private static bool IsInvalid(Guid input) => input == Guid.Empty;
        private bool IsInvalid(DateTimeOffset input) => input == default;

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
            }
        }
    }
}
