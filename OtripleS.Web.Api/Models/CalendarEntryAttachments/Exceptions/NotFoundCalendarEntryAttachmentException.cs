//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class NotFoundCalendarEntryAttachmentException : Exception
    {
        public NotFoundCalendarEntryAttachmentException(Guid calendarEntryId, Guid attachmentId)
          : base($"Couldn't find CalendarEntryAttachment with calendarEntryId: " +
                    $"{calendarEntryId} " +
                    $"and attachmentId: {attachmentId}.")
        { }
    }
}
