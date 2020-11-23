// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Calendars;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Calendar> InsertCalendarAsync(Calendar calendar);
        IQueryable<Calendar> SelectAllCalendars();
        ValueTask<Calendar> SelectCalendarByIdAsync(Guid calendarId);
        ValueTask<Calendar> UpdateCalendarAsync(Calendar calendar);
        ValueTask<Calendar> DeleteCalendarAsync(Calendar calendar);
    }
}
