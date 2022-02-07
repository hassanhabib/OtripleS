//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomGuardianId = Guid.Empty;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianAttachmentInputException = new InvalidGuardianAttachmentException(
                parameterName: nameof(GuardianAttachment.GuardianId),
                parameterValue: inputGuardianId);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentInputException);

            // when
            ValueTask<GuardianAttachment> removeGuardianAttachmentTask =
                this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() => removeGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.Empty;
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianAttachmentInputException = new InvalidGuardianAttachmentException(
                parameterName: nameof(GuardianAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentInputException);

            // when
            ValueTask<GuardianAttachment> removeGuardianAttachmentTask =
                this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() => removeGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageGuardianAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment(randomDateTime);
            Guid inputAttachmentId = randomGuardianAttachment.AttachmentId;
            Guid inputGuardianId = randomGuardianAttachment.GuardianId;
            GuardianAttachment nullStorageGuardianAttachment = null;

            var notFoundGuardianAttachmentException =
                new NotFoundGuardianAttachmentException(inputGuardianId, inputAttachmentId);

            var expectedSemesterCourseValidationException =
                new GuardianAttachmentValidationException(notFoundGuardianAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId))
                    .ReturnsAsync(nullStorageGuardianAttachment);

            // when
            ValueTask<GuardianAttachment> removeGuardianAttachmentTask =
                this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                removeGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}