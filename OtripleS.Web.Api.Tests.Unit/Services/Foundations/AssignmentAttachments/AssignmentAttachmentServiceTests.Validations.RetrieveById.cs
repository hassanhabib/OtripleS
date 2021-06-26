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
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenAssignmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomAssignmentId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid invalidAssignmentId = randomAssignmentId;

            var invalidAssignmentAttachmentInputException = new InvalidAssignmentAttachmentException(
                parameterName: nameof(AssignmentAttachment.AssignmentId),
                parameterValue: invalidAssignmentId);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(invalidAssignmentAttachmentInputException);

            // when
            ValueTask<AssignmentAttachment> retrieveAssignmentAttachmentTask =
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    invalidAssignmentId,
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                retrieveAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid randomAssignmentId = Guid.NewGuid();
            Guid invalidAttachmentId = randomAttachmentId;
            Guid inputAssignmentId = randomAssignmentId;

            var invalidAssignmentAttachmentInputException = new InvalidAssignmentAttachmentException(
                parameterName: nameof(AssignmentAttachment.AttachmentId),
                parameterValue: invalidAttachmentId);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(invalidAssignmentAttachmentInputException);

            // when
            ValueTask<AssignmentAttachment> retrieveAssignmentAttachmentTask =
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    inputAssignmentId,
                    invalidAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                retrieveAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageAssignmentAttachmentIsInvalidAndLogItAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            Guid inputAttachmentId = randomAssignmentAttachment.AttachmentId;
            Guid inputAssignmentId = randomAssignmentAttachment.AssignmentId;
            AssignmentAttachment nullStorageAssignmentAttachment = null;

            var notFoundAssignmentAttachmentException =
                new NotFoundAssignmentAttachmentException(inputAssignmentId, inputAttachmentId);

            var expectedAttachmentValidationException =
                new AssignmentAttachmentValidationException(notFoundAssignmentAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectAssignmentAttachmentByIdAsync(inputAssignmentId, inputAttachmentId))
                    .ReturnsAsync(nullStorageAssignmentAttachment);

            // when
            ValueTask<AssignmentAttachment> retrieveAssignmentAttachmentTask =
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    inputAssignmentId,
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                retrieveAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
