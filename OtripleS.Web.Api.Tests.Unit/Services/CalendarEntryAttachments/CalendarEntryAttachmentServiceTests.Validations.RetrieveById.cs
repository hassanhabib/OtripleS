//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenCalendarEntryIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomCalendarEntryId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCalendarEntryId = randomCalendarEntryId;

            var invalidCalendarEntryAttachmentInputException = new InvalidCalendarEntryAttachmentException(
                parameterName: nameof(CalendarEntryAttachment.CalendarEntryId),
                parameterValue: inputCalendarEntryId);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(invalidCalendarEntryAttachmentInputException);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() => retrieveCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        } 

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCalendarEntryId = randomCalendarEntryId;

            var invalidCalendarEntryAttachmentInputException = new InvalidCalendarEntryAttachmentException(
                parameterName: nameof(CalendarEntryAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(invalidCalendarEntryAttachmentInputException);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(
                    inputCalendarEntryId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() => 
                retrieveCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task 
            ShouldThrowValidationExceptionOnRetrieveWhenStorageCalendarEntryAttachmentIsInvalidAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            Guid inputAttachmentId = randomCalendarEntryAttachment.AttachmentId;
            Guid inputCalendarEntryId = randomCalendarEntryAttachment.CalendarEntryId;
            CalendarEntryAttachment nullStorageCalendarEntryAttachment = null;

            var notFoundCalendarEntryAttachmentException =
                new NotFoundCalendarEntryAttachmentException(inputCalendarEntryId, inputAttachmentId);

            var expectedAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(notFoundCalendarEntryAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId))
                    .ReturnsAsync(nullStorageCalendarEntryAttachment);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(
                    inputCalendarEntryId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() =>
                retrieveCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                    Times.Once); 
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
