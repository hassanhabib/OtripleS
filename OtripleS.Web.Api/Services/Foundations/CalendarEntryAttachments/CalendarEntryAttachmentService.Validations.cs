// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private static void ValidateCalendarEntryAttachmentOnCreate(CalendarEntryAttachment calendarEntryAttachment)
        {
            ValidateCalendarEntryAttachmentIsNull(calendarEntryAttachment);

            ValidateCalendarEntryAttachmentIds(
                calendarEntryAttachment.CalendarEntryId,
                calendarEntryAttachment.AttachmentId);
        }

        private static void ValidateCalendarEntryAttachmentIsNull(CalendarEntryAttachment calendarEntryAttachment)
        {
            if (calendarEntryAttachment is null)
            {
                throw new NullCalendarEntryAttachmentException();
            }
        }

        private static void ValidateCalendarEntryAttachmentIds(Guid calendarEntryId, Guid attachmentId)
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

        private void ValidateStorageCalendarEntryAttachments(
            IQueryable<CalendarEntryAttachment> storageCalendarEntryAttachments)
        {
            if (!storageCalendarEntryAttachments.Any())
            {
                this.loggingBroker.LogWarning("No calendarentry attachments found in storage.");
            }
        }
    }
}