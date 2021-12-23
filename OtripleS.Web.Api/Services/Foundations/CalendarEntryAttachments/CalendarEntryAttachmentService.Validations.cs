// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private static void ValidateCalendarEntryAttachmentOnAdd(CalendarEntryAttachment calendarEntryAttachment)
        {
            ValidateCalendarEntryAttachmentIsNull(calendarEntryAttachment);

            Validate(
                (Rule: IsInvalid(calendarEntryAttachment.AttachmentId),
                Parameter: nameof(CalendarEntryAttachment.AttachmentId)),

                (Rule: IsInvalid(calendarEntryAttachment.CalendarEntryId),
                Parameter: nameof(CalendarEntryAttachment.CalendarEntryId)));
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
            Validate(
               (Rule: IsInvalid(attachmentId),
               Parameter: nameof(CalendarEntryAttachment.AttachmentId)),

               (Rule: IsInvalid(calendarEntryId),
               Parameter: nameof(CalendarEntryAttachment.CalendarEntryId)));
        }

        private static void ValidateStorageCalendarEntryAttachment(
            CalendarEntryAttachment storageCalendarEntryAttachment,
            Guid calendarEntryId, Guid attachmentId)
        {
            if (storageCalendarEntryAttachment == null)
                throw new NotFoundCalendarEntryAttachmentException(calendarEntryId, attachmentId);
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCalendarEntryAttachmentException = new InvalidCalendarEntryAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCalendarEntryAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCalendarEntryAttachmentException.ThrowIfContainsErrors();
        }
    }
}