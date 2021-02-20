// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenExamIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomExamId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputExamId = randomExamId;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException(
                parameterName: nameof(ExamAttachment.ExamId),
                parameterValue: inputExamId);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentInputException);

            // when
            ValueTask<ExamAttachment> removeExamAttachmentTask =
                this.examAttachmentService.RemoveExamAttachmentByIdAsync(inputExamId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                removeExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomExamId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputExamId = randomExamId;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException(
                parameterName: nameof(ExamAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentInputException);

            // when
            ValueTask<ExamAttachment> removeExamAttachmentTask =
                this.examAttachmentService.RemoveExamAttachmentByIdAsync(inputExamId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                removeExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageExamAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment(randomDateTime);
            Guid inputAttachmentId = randomExamAttachment.AttachmentId;
            Guid inputExamId = randomExamAttachment.ExamId;
            ExamAttachment nullStorageExamAttachment = null;

            var notFoundExamAttachmentException =
                new NotFoundExamAttachmentException(inputExamId, inputAttachmentId);

            var expectedExamValidationException =
                new ExamAttachmentValidationException(notFoundExamAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectExamAttachmentByIdAsync(inputExamId, inputAttachmentId))
                    .ReturnsAsync(nullStorageExamAttachment);

            // when
            ValueTask<ExamAttachment> removeExamAttachmentTask =
                this.examAttachmentService.RemoveExamAttachmentByIdAsync(inputExamId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                removeExamAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
