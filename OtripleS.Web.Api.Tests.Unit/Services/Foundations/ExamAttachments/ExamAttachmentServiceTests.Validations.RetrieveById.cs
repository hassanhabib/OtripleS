//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenExamIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid randomExamId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid invalidExamId = randomExamId;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException();

            invalidExamAttachmentInputException.AddData(
                key: nameof(ExamAttachment.ExamId),
                values: "Id is required");

            invalidExamAttachmentInputException.AddData(
                key: nameof(ExamAttachment.AttachmentId),
                values: "Id is required");

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentInputException);

            // when
            ValueTask<ExamAttachment> retrieveExamAttachmentTask =
                this.examAttachmentService.RetrieveExamAttachmentByIdAsync(
                    invalidExamId,
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                retrieveExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageExamAttachmentIsInvalidAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            Guid inputAttachmentId = randomExamAttachment.AttachmentId;
            Guid inputExamId = randomExamAttachment.ExamId;
            ExamAttachment nullStorageExamAttachment = null;

            var notFoundExamAttachmentException =
                new NotFoundExamAttachmentException(inputExamId, inputAttachmentId);

            var expectedAttachmentValidationException =
                new ExamAttachmentValidationException(notFoundExamAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectExamAttachmentByIdAsync(inputExamId, inputAttachmentId))
                    .ReturnsAsync(nullStorageExamAttachment);

            // when
            ValueTask<ExamAttachment> retrieveExamAttachmentTask =
                this.examAttachmentService.RetrieveExamAttachmentByIdAsync(
                    inputExamId,
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                retrieveExamAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(inputExamId, inputAttachmentId),
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
