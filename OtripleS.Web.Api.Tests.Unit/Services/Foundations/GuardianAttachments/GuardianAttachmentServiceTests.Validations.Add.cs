//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianAttachmentIsNullAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = default;
            GuardianAttachment nullGuardianAttachment = randomGuardianAttachment;
            var nullGuardianAttachmentException = new NullGuardianAttachmentException();

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(nullGuardianAttachmentException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(nullGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment inputGuardianAttachment = randomGuardianAttachment;
            inputGuardianAttachment.GuardianId = default;

            var invalidGuardianAttachmentInputException = new InvalidGuardianAttachmentException(
                parameterName: nameof(GuardianAttachment.GuardianId),
                parameterValue: inputGuardianAttachment.GuardianId);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentInputException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(inputGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment inputGuardianAttachment = randomGuardianAttachment;
            inputGuardianAttachment.AttachmentId = default;

            var invalidGuardianAttachmentInputException = new InvalidGuardianAttachmentException(
                parameterName: nameof(GuardianAttachment.AttachmentId),
                parameterValue: inputGuardianAttachment.AttachmentId);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentInputException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(inputGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment alreadyExistsGuardianAttachment = randomGuardianAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsGuardianAttachmentException =
                new AlreadyExistsGuardianAttachmentException(duplicateKeyException);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(alreadyExistsGuardianAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAttachmentAsync(alreadyExistsGuardianAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(alreadyExistsGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(alreadyExistsGuardianAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment invalidGuardianAttachment = randomGuardianAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidGuardianAttachmentReferenceException =
                new InvalidGuardianAttachmentReferenceException(foreignKeyConstraintConflictException);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAttachmentAsync(invalidGuardianAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(invalidGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(invalidGuardianAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
