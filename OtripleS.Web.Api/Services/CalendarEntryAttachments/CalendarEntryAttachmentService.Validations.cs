// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private void ValidateCalendarEntryAttachmentIdIsNull(Guid calendarEntryId, Guid attachmentId)
        {
            if (calendarEntryId == default)
            {
                throw new InvalidCalendarEntryAttachmentException(
                    parameterName: nameof(CalendarEntryAttachment.CalendarEntryId),
                    parameterValue: calendarEntryId);
            } 
        }
    }
}
