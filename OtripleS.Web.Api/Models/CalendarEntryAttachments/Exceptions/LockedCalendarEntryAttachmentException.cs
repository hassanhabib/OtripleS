using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class LockedCalendarEntryAttachmentException : Exception
    {
        public LockedCalendarEntryAttachmentException(Exception innerException)
            : base("Locked CalendarEntryAttachment record exception, please try again later.", innerException)
        {

        }
    }
}
