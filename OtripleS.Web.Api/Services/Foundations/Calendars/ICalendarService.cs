// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Calendars;

namespace OtripleS.Web.Api.Services.Foundations.Calendars
{
    public interface ICalendarService
    {
        ValueTask<Calendar> AddCalendarAsync(Calendar calendar);
        IQueryable<Calendar> RetrieveAllCalendars();
        ValueTask<Calendar> RetrieveCalendarByIdAsync(Guid calendarId);
        ValueTask<Calendar> ModifyCalendarAsync(Calendar calendar);
        ValueTask<Calendar> RemoveCalendarByIdAsync(Guid calendarId);
    }
}
