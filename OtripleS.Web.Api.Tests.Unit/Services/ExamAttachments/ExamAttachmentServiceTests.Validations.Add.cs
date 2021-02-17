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

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamAttachmentIsNullAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = default;
            ExamAttachment nullExamAttachment = randomExamAttachment;
            var nullExamAttachmentException = new NullExamAttachmentException();

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(nullExamAttachmentException);

            // when
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(nullExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamIdIsInvalidAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment inputExamAttachment = randomExamAttachment;
            inputExamAttachment.ExamId = default;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException(
                parameterName: nameof(ExamAttachment.ExamId),
                parameterValue: inputExamAttachment.ExamId);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentInputException);

            // when
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(inputExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment inputExamAttachment = randomExamAttachment;
            inputExamAttachment.AttachmentId = default;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException(
                parameterName: nameof(ExamAttachment.AttachmentId),
                parameterValue: inputExamAttachment.AttachmentId);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentInputException);

            // when
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(inputExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
