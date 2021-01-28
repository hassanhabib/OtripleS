using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class InvalidCalendarEntryAttachmentException : Exception
    {
        public InvalidCalendarEntryAttachmentException(string parameterName, object parameterValue)
            : base($"Invalid CalendarEntryAttachment, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
