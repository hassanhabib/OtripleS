using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class NullCalendarEntryAttachmentException : Exception
    {
        public NullCalendarEntryAttachmentException()
            : base("The calendarEntry attachment is null")
        {

        }
    }
}
