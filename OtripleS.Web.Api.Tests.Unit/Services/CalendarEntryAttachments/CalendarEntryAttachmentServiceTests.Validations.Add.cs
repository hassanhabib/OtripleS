//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarEntryAttachmentIsNullAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = default;
            CalendarEntryAttachment nullCalendarEntryAttachment = randomCalendarEntryAttachment;
            var nullCalendarEntryAttachmentException = new NullCalendarEntryAttachmentException();

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(nullCalendarEntryAttachmentException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(nullCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
