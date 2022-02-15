//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using Xeptions;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class InvalidCalendarEntryAttachmentException : Xeption
    {
        public InvalidCalendarEntryAttachmentException()
            : base("Invalid calendar entry attachment error occurred. Please fix the errors and try again.")
        { }
    }
}
