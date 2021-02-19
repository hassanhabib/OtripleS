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
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenExamIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomExamId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid invalidExamId = randomExamId;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException(
                parameterName: nameof(ExamAttachment.ExamId),
                parameterValue: invalidExamId);

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
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid randomExamId = Guid.NewGuid();
            Guid invalidAttachmentId = randomAttachmentId;
            Guid inputExamId = randomExamId;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException(
                parameterName: nameof(ExamAttachment.AttachmentId),
                parameterValue: invalidAttachmentId);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentInputException);

            // when
            ValueTask<ExamAttachment> retrieveExamAttachmentTask =
                this.examAttachmentService.RetrieveExamAttachmentByIdAsync(
                    inputExamId,
                    invalidAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                retrieveExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
