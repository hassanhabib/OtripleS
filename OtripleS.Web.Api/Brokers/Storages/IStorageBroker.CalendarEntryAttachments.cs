// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<CalendarEntryAttachment> InsertCalendarEntryAttachmentAsync(
          CalendarEntryAttachment calendarEntryAttachment);

        IQueryable<CalendarEntryAttachment> SelectAllCalendarEntryAttachments();

        ValueTask<CalendarEntryAttachment> SelectCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId,
            Guid attachmentId);

        ValueTask<CalendarEntryAttachment> UpdateCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment);

        ValueTask<CalendarEntryAttachment> DeleteCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment);
    }
}