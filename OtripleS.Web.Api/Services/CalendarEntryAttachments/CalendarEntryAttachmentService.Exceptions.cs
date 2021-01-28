//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService
    {
        private delegate ValueTask<CalendarEntryAttachment> ReturningCalendarEntryAttachmentFunction();

        private async ValueTask<CalendarEntryAttachment> TryCatch(
            ReturningCalendarEntryAttachmentFunction returningCalendarEntryAttachmentFunction)
        {
            try
            {
                return await returningCalendarEntryAttachmentFunction();
            }
            catch (InvalidCalendarEntryAttachmentException invalidCalendarEntryAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidCalendarEntryAttachmentInputException);
            }
        }

        private CalendarEntryAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var calendarEntryAttachmentValidationException = new CalendarEntryAttachmentValidationException(exception);
            this.loggingBroker.LogError(calendarEntryAttachmentValidationException);

            return calendarEntryAttachmentValidationException;
        }

    }
}
