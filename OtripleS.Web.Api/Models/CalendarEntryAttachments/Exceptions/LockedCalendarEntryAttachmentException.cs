// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class LockedCalendarEntryAttachmentException : Exception
    {
        public LockedCalendarEntryAttachmentException(Exception innerException)
            : base(message: "Locked calendar entry attachment record exception, please try again later.",
                  innerException)
        { }
    }
}
