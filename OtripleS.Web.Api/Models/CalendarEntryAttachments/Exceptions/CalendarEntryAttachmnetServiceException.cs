namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    using System;

    public class CalendarEntryAttachmnetServiceException : Exception
    {
        public CalendarEntryAttachmnetServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
