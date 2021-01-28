using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class AlreadyExistsCalendarEntryAttachmentException : Exception
    {
        public AlreadyExistsCalendarEntryAttachmentException(Exception innerException)
             : base("CalendarEntry attachment with the same Id already exist.", innerException)
        { }
    }
}
