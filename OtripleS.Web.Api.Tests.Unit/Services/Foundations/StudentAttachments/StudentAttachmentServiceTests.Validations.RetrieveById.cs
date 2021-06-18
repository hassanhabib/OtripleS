//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Foundations.StudentAttachments;
using OtripleS.Web.Api.Models.Foundations.StudentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomStudentId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentAttachmentInputException = new InvalidStudentAttachmentException(
                parameterName: nameof(StudentAttachment.StudentId),
                parameterValue: inputStudentId);

            var expectedStudentAttachmentValidationException =
                new StudentAttachmentValidationException(invalidStudentAttachmentInputException);

            // when
            ValueTask<StudentAttachment> actualStudentAttachmentTask =
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() => actualStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid randomStudentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentAttachmentInputException = new InvalidStudentAttachmentException(
                parameterName: nameof(StudentAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedStudentAttachmentValidationException =
                new StudentAttachmentValidationException(invalidStudentAttachmentInputException);

            // when
            ValueTask<StudentAttachment> actualStudentAttachmentTask =
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() => actualStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageStudentAttachmentIsInvalidAndLogItAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            Guid inputAttachmentId = randomStudentAttachment.AttachmentId;
            Guid inputStudentId = randomStudentAttachment.StudentId;
            StudentAttachment nullStorageStudentAttachment = null;

            var notFoundStudentAttachmentException =
                new NotFoundStudentAttachmentException(inputStudentId, inputAttachmentId);

            var expectedAttachmentValidationException =
                new StudentAttachmentValidationException(notFoundStudentAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId))
                    .ReturnsAsync(nullStorageStudentAttachment);

            // when
            ValueTask<StudentAttachment> actualStudentAttachmentRetrieveTask =
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() =>
                actualStudentAttachmentRetrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
