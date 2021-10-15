// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntryAttachments
{
    public interface ICalendarEntryAttachmentService
    {
        ValueTask<CalendarEntryAttachment> AddCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment);

        IQueryable<CalendarEntryAttachment> RetrieveAllCalendarEntryAttachments();

        ValueTask<CalendarEntryAttachment> RetrieveCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId,
            Guid attachmentId);

        ValueTask<CalendarEntryAttachment> RemoveCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId,
            Guid attachmentId);
    }
}
