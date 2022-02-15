// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamAttachmentIsNullAndLogItAsync()
        {
            // given
            ExamAttachment invalidExamAttachment = null;

            var nullExamAttachmentException = new NullExamAttachmentException();

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(nullExamAttachmentException);

            // when
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(invalidExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenIdsAreInvalidAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment inputExamAttachment = randomExamAttachment;
            inputExamAttachment.ExamId = default;
            inputExamAttachment.AttachmentId = default;

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
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(inputExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedExamAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenExamAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment alreadyExistsExamAttachment = randomExamAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsExamAttachmentException =
                new AlreadyExistsExamAttachmentException(duplicateKeyException);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(alreadyExistsExamAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamAttachmentAsync(alreadyExistsExamAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(alreadyExistsExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedExamAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(alreadyExistsExamAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment invalidExamAttachment = randomExamAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidExamAttachmentReferenceException =
                new InvalidExamAttachmentReferenceException(foreignKeyConstraintConflictException);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamAttachmentAsync(invalidExamAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(invalidExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedExamAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(invalidExamAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
