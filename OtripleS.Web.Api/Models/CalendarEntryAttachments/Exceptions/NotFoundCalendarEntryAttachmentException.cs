using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class NotFoundCalendarEntryAttachmentException : Exception
    {
        public NotFoundCalendarEntryAttachmentException(Guid calendarEntryId, Guid attachmentId)
            : base($"Couldn't find calendarEntry attachment with calendarEntryId: {calendarEntryId} " + 
                  $" and attachmentId: {attachmentId}." ) { }
    }
}
