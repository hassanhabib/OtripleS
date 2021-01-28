// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using System;
using System.Linq;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        public void ValidateCalendarEntryAttachmentOnCreate(CalendarEntryAttachment CalendarEntryAttachment)
        {
            ValidateCalendarEntryAttachmentIsNull(CalendarEntryAttachment);
            ValidateCalendarEntryAttachmentIdIsNull(CalendarEntryAttachment.CalendarEntryId, CalendarEntryAttachment.AttachmentId);
        }

        private void ValidateCalendarEntryAttachmentIsNull(CalendarEntryAttachment CalendarEntryAttachment)
        {
            if (CalendarEntryAttachment is null)
            {
                throw new NullCalendarEntryAttachmentException();
            }
        }

        private void ValidateCalendarEntryAttachmentIdIsNull(Guid guardianId, Guid attachmentId)
        {
            if (guardianId == default)
            {
                throw new InvalidCalendarEntryAttachmentException(
                    parameterName: nameof(CalendarEntryAttachment.CalendarEntryId),
                    parameterValue: guardianId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidCalendarEntryAttachmentException(
                    parameterName: nameof(CalendarEntryAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageCalendarEntryAttachment(
            CalendarEntryAttachment storageStudentAttachment,
            Guid guardianId, Guid attachmentId)
        {
            if (storageStudentAttachment == null)
            {
                throw new NotFoundCalendarEntryAttachmentException(guardianId, attachmentId);
            }
        }

        private void ValidateStorageCalendarEntryAttachments(IQueryable<CalendarEntryAttachment> storageCalendarEntryAttachments)
        {
            if (storageCalendarEntryAttachments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No calendarEntry attachments found in storage.");
            }
        }
    }
}
