// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.CalendarEntriesAttachments;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string CalendarEntryAttachmentsRelativeUrl = "api/calendarentriesattachments";

        public async ValueTask<CalendarEntryAttachment> PostCalendarEntryAttachmentAsync(CalendarEntryAttachment calendarEntryAttachment) =>
            await this.apiFactoryClient.PostContentAsync(CalendarEntryAttachmentsRelativeUrl, calendarEntryAttachment);

        public async ValueTask<CalendarEntryAttachment> GetCalendarEntryAttachmentByIdsAsync(Guid calendarEntryId, Guid attachmentId) =>
            await this.apiFactoryClient.GetContentAsync<CalendarEntryAttachment>(
                $"{CalendarEntryAttachmentsRelativeUrl}/calendarentries/{calendarEntryId}/attachments/{attachmentId}");

        public async ValueTask<CalendarEntryAttachment> DeleteCalendarEntryAttachmentByIdsAsync(Guid calendarEntryId, Guid attachmentId) =>
            await this.apiFactoryClient.DeleteContentAsync<CalendarEntryAttachment>(
                $"{CalendarEntryAttachmentsRelativeUrl}/calendarentries/{calendarEntryId}/attachments/{attachmentId}");

        public async ValueTask<List<CalendarEntryAttachment>> GetAllCalendarEntryAttachmentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<CalendarEntryAttachment>>($"{CalendarEntryAttachmentsRelativeUrl}/");
    }
}
