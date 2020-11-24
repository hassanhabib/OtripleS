// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;

namespace OtripleS.Web.Api.Services.Calendars
{
    public partial class CalendarService
    {
        private void ValidateCalendarId(Guid calendarId)
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
    }
}
