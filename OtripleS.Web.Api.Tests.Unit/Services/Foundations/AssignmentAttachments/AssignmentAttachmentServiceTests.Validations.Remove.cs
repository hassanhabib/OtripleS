//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenAssignmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomAssignmentId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputAssignmentId = randomAssignmentId;

            var invalidAssignmentAttachmentInputException = new InvalidAssignmentAttachmentException(
                parameterName: nameof(AssignmentAttachment.AssignmentId),
                parameterValue: inputAssignmentId);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(invalidAssignmentAttachmentInputException);

            // when
            ValueTask<AssignmentAttachment> removeAssignmentAttachmentTask =
                this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(
                    inputAssignmentId,
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                removeAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomAssignmentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputAssignmentId = randomAssignmentId;

            var invalidAssignmentAttachmentInputException = new InvalidAssignmentAttachmentException(
                parameterName: nameof(AssignmentAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(invalidAssignmentAttachmentInputException);

            // when
            ValueTask<AssignmentAttachment> removeAssignmentAttachmentTask =
                this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(
                    inputAssignmentId,
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                removeAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageAssignmentAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment(randomDateTime);
            Guid inputAttachmentId = randomAssignmentAttachment.AttachmentId;
            Guid inputAssignmentId = randomAssignmentAttachment.AssignmentId;
            AssignmentAttachment nullStorageAssignmentAttachment = null;

            var notFoundAssignmentAttachmentException =
                new NotFoundAssignmentAttachmentException(inputAssignmentId, inputAttachmentId);

            var expectedAssignmentValidationException =
                new AssignmentAttachmentValidationException(notFoundAssignmentAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectAssignmentAttachmentByIdAsync(inputAssignmentId, inputAttachmentId))
                    .ReturnsAsync(nullStorageAssignmentAttachment);

            // when
            ValueTask<AssignmentAttachment> removeAssignmentAttachmentTask =
                this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(
                    inputAssignmentId,
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                removeAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
