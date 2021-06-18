// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Foundations.CalendarEntryAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<CalendarEntryAttachment> InsertCalendarEntryAttachmentAsync(
           CalendarEntryAttachment calendarEntryAttachment);

        public IQueryable<CalendarEntryAttachment> SelectAllCalendarEntryAttachments();

        public ValueTask<CalendarEntryAttachment> SelectCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId,
            Guid attachmentId);

        public ValueTask<CalendarEntryAttachment> UpdateCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment);

        public ValueTask<CalendarEntryAttachment> DeleteCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment);
    }
}
