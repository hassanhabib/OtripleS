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
        }

        private static void ValidateCalendarEntryIsNotNull(CalendarEntry CalendarEntry)
        {
            if (CalendarEntry is null)
            {
                throw new NullCalendarEntryException();
            }
        }
    }
}
