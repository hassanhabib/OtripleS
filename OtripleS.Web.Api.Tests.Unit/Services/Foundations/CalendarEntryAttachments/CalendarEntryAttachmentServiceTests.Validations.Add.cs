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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarEntryAttachmentIsNullAndLogItAsync()
        {
            // given
            CalendarEntryAttachment nullCalendarEntryAttachment = default;
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarEntryIdIsInvalidAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            inputCalendarEntryAttachment.CalendarEntryId = default;

            var invalidCalendarEntryAttachmentInputException = new InvalidCalendarEntryAttachmentException(
                parameterName: nameof(CalendarEntryAttachment.CalendarEntryId),
                parameterValue: inputCalendarEntryAttachment.CalendarEntryId);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(invalidCalendarEntryAttachmentInputException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            inputCalendarEntryAttachment.AttachmentId = default;

            var invalidCalendarEntryAttachmentInputException = new InvalidCalendarEntryAttachmentException(
                parameterName: nameof(CalendarEntryAttachment.AttachmentId),
                parameterValue: inputCalendarEntryAttachment.AttachmentId);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(invalidCalendarEntryAttachmentInputException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarEntryAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment alreadyExistsCalendarEntryAttachment = randomCalendarEntryAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCalendarEntryAttachmentException =
                new AlreadyExistsCalendarEntryAttachmentException(duplicateKeyException);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(alreadyExistsCalendarEntryAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(alreadyExistsCalendarEntryAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(alreadyExistsCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment someCalendarEntryAttachment = randomCalendarEntryAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidCalendarEntryAttachmentReferenceException =
                new InvalidCalendarEntryAttachmentReferenceException(foreignKeyConstraintConflictException);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(invalidCalendarEntryAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(someCalendarEntryAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(someCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
