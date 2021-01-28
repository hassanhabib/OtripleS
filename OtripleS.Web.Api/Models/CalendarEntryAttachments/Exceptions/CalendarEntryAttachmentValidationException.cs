using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class CalendarEntryAttachmentValidationException : Exception
    {
        public CalendarEntryAttachmentValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
