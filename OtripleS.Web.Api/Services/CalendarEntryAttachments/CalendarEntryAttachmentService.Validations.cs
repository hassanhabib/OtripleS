// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using System;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private void ValidateCalendarEntryAttachmentIds(Guid calendarEntryId, Guid attachmentId)
        {
            if (calendarEntryId == default)
            {
                throw new InvalidCalendarEntryAttachmentException(
                    parameterName: nameof(CalendarEntryAttachment.CalendarEntryId),
                    parameterValue: calendarEntryId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidCalendarEntryAttachmentException(
                    parameterName: nameof(CalendarEntryAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageCalendarEntryAttachment(
            CalendarEntryAttachment storageCalendarEntryAttachment,
            Guid calendarEntryId, Guid attachmentId)
        {
            if (storageCalendarEntryAttachment == null)
                throw new NotFoundCalendarEntryAttachmentException(calendarEntryId, attachmentId);
        }
    }
}
