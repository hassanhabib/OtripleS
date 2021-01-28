using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public interface ICalendarEntryAttachmentService
    {
        ValueTask<CalendarEntryAttachment> RemoveCalendarEntryAttachmentByIdAsync(Guid calendarEntryId, Guid attachmentId);
    }
}
