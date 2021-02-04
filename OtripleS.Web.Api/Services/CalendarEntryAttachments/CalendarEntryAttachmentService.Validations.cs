// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using System;
using System.Linq;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private void ValidateCalendarEntryAttachmentOnCreate(CalendarEntryAttachment calendarEntryAttachment)
        {
            ValidateCalendarEntryAttachmentIsNull(calendarEntryAttachment);
            
            ValidateCalendarEntryAttachmentIds(
                calendarEntryAttachment.CalendarEntryId, 
                calendarEntryAttachment.AttachmentId);
        }

        private void ValidateCalendarEntryAttachmentIsNull(CalendarEntryAttachment calendarEntryAttachment)
        {
            if (calendarEntryAttachment is null)
            {
                throw new NullCalendarEntryAttachmentException();
            }
        }

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

        private void ValidateStorageCalendarEntryAttachments(
            IQueryable<CalendarEntryAttachment> storageCalendarEntryAttachments)
        {
            if (storageCalendarEntryAttachments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No calendarentry attachments found in storage.");
            }
        }
    }
}