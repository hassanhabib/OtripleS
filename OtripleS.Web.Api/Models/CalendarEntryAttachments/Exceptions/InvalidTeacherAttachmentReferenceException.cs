// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class InvalidCalendarEntryAttachmentReferenceException : Exception
    {
        public InvalidCalendarEntryAttachmentReferenceException(Exception innerException)
            : base(message:  "Invalid calendar entry attachment reference error occurred.", innerException) { }
    }
}
