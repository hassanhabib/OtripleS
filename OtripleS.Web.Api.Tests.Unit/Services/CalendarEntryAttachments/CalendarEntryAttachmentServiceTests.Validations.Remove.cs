//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenCalendarEntryIdIsInvalidAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            Guid randomCalendarEntryId = default;
            var inputAttachmentId = randomAttachmentId;
            var inputCalendarEntryId = randomCalendarEntryId;

            var invalidCalendarEntryAttachmentInputException = new InvalidCalendarEntryAttachmentException(
                parameterName: nameof(CalendarEntryAttachment.CalendarEntryId),
                parameterValue: inputCalendarEntryId);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(invalidCalendarEntryAttachmentInputException);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() => removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomAttachmentId = default;
            var randomCalendarEntryId = Guid.NewGuid();
            var inputAttachmentId = randomAttachmentId;
            var inputCalendarEntryId = randomCalendarEntryId;

            var invalidCalendarAttachmentInputException = new InvalidCalendarEntryAttachmentException(
                parameterName: nameof(CalendarEntryAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedCalendarAttachmentValidation =
                new CalendarEntryAttachmentValidationException(invalidCalendarAttachmentInputException);

            //when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
               this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId);


            //then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() => removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarAttachmentValidation))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageGuardianAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CalendarEntryAttachment randomGuardianAttachment = CreateRandomCalendarEntryAttachment(randomDateTime);
            Guid inputAttachmentId = randomGuardianAttachment.AttachmentId;
            Guid inputCalendarEntryId = randomGuardianAttachment.CalendarEntryId;
            CalendarEntryAttachment nullStorageCalendarEntryAttachment = null;

            var notFoundCalendarEntryAttachmentException =
               new NotFoundCalendarEntryAttachmentException(inputCalendarEntryId, inputAttachmentId);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(notFoundCalendarEntryAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId))
                    .ReturnsAsync(nullStorageCalendarEntryAttachment);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
               this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId);

            //then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() =>
               removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}