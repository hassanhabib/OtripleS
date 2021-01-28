using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class CalendarEntryAttachmentServiceException : Exception
    {
        public CalendarEntryAttachmentServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException)
        {

        }
    }
}
