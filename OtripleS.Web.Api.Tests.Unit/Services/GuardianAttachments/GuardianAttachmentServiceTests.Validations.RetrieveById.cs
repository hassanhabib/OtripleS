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

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomGuardianId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianAttachmentInputException = new InvalidGuardianAttachmentException(
                parameterName: nameof(GuardianAttachment.GuardianId),
                parameterValue: inputGuardianId);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentInputException);

            // when
            ValueTask<GuardianAttachment> actualGuardianAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() => actualGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianAttachmentInputException = new InvalidGuardianAttachmentException(
                parameterName: nameof(GuardianAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentInputException);

            // when
            ValueTask<GuardianAttachment> actualGuardianAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() => actualGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageGuardianAttachmentIsInvalidAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            Guid inputAttachmentId = randomGuardianAttachment.AttachmentId;
            Guid inputGuardianId = randomGuardianAttachment.GuardianId;
            GuardianAttachment nullStorageGuardianAttachment = null;

            var notFoundGuardianAttachmentException =
                new NotFoundGuardianAttachmentException(inputGuardianId, inputAttachmentId);

            var expectedAttachmentValidationException =
                new GuardianAttachmentValidationException(notFoundGuardianAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId))
                    .ReturnsAsync(nullStorageGuardianAttachment);

            // when
            ValueTask<GuardianAttachment> actualGuardianAttachmentRetrieveTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                actualGuardianAttachmentRetrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
