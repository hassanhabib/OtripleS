//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAssignmentAttachmentIsNullAndLogItAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = default;
            AssignmentAttachment nullAssignmentAttachment = randomAssignmentAttachment;
            var nullAssignmentAttachmentException = new NullAssignmentAttachmentException();

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(nullAssignmentAttachmentException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(nullAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAssignmentIdIsInvalidAndLogItAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment inputAssignmentAttachment = randomAssignmentAttachment;
            inputAssignmentAttachment.AssignmentId = default;

            var invalidAssignmentAttachmentInputException = new InvalidAssignmentAttachmentException(
                parameterName: nameof(AssignmentAttachment.AssignmentId),
                parameterValue: inputAssignmentAttachment.AssignmentId);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(invalidAssignmentAttachmentInputException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(inputAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment inputAssignmentAttachment = randomAssignmentAttachment;
            inputAssignmentAttachment.AttachmentId = default;

            var invalidAssignmentAttachmentInputException = new InvalidAssignmentAttachmentException(
                parameterName: nameof(AssignmentAttachment.AttachmentId),
                parameterValue: inputAssignmentAttachment.AttachmentId);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(invalidAssignmentAttachmentInputException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(inputAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAssignmentAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment alreadyExistsAssignmentAttachment = randomAssignmentAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAssignmentAttachmentException =
                new AlreadyExistsAssignmentAttachmentException(duplicateKeyException);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(alreadyExistsAssignmentAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAttachmentAsync(alreadyExistsAssignmentAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(alreadyExistsAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(alreadyExistsAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment invalidAssignmentAttachment = randomAssignmentAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidAssignmentAttachmentReferenceException =
                new InvalidAssignmentAttachmentReferenceException(foreignKeyConstraintConflictException);

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(invalidAssignmentAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAttachmentAsync(invalidAssignmentAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(invalidAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(invalidAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
