using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class InvalidCalendarEntryAttachmentReferenceException : Exception
    {
        public InvalidCalendarEntryAttachmentReferenceException(Exception innerException)
            : base("Invalid calenderEntry attachment reference error occurred.", innerException)
        {

        }
    }
}
