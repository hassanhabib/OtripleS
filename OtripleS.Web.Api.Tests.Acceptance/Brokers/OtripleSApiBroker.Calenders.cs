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

        public async ValueTask<Calendar> GetCalenderByIdAsync(Guid calenderId) =>
            await this.apiFactoryClient.GetContentAsync<Calendar>($"{CalendarRelativeUrl}/{calenderId}");
        public async ValueTask<Calendar> PutCalendarAsync(Calendar calendar) =>
         await this.apiFactoryClient.PutContentAsync(CalendarRelativeUrl, calendar);
        public async ValueTask<Calendar> DeleteCalenderByIdAsync(Guid calenderId) =>
            await this.apiFactoryClient.DeleteContentAsync<Calendar>($"{CalendarRelativeUrl}/{calenderId}");

        public async ValueTask<List<Calendar>> GetAllCalendersAsync() =>
          await  this.apiFactoryClient.GetContentAsync<List<Calendar>>($"{CalendarRelativeUrl}/");


    }
}
