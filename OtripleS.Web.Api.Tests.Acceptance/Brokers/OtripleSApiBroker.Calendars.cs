// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Calendars;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string CalendarRelativeUrl = "api/Calendars";

        public async ValueTask<Calendar> PostCalendarAsync(Calendar calendar) =>
            await this.apiFactoryClient.PostContentAsync(CalendarRelativeUrl, calendar);

        public async ValueTask<Calendar> GetCalendarByIdAsync(Guid calendarId) =>
            await this.apiFactoryClient.GetContentAsync<Calendar>($"{CalendarRelativeUrl}/{calendarId}");

        public async ValueTask<Calendar> PutCalendarAsync(Calendar calendar) =>
            await this.apiFactoryClient.PutContentAsync(CalendarRelativeUrl, calendar);

        public async ValueTask<Calendar> DeleteCalendarByIdAsync(Guid calendarId) =>
            await this.apiFactoryClient.DeleteContentAsync<Calendar>($"{CalendarRelativeUrl}/{calendarId}");

        public async ValueTask<List<Calendar>> GetAllCalendarsAsync() =>
            await  this.apiFactoryClient.GetContentAsync<List<Calendar>>($"{CalendarRelativeUrl}/");
    }
}
