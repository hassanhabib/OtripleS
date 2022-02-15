//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStudentIdIsInvalidAndLogItAsync()
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
            ValueTask<StudentAttachment> removeStudentAttachmentTask =
                this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() => removeStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenAttachmentIdIsInvalidAndLogItAsync()
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
            ValueTask<StudentAttachment> removeStudentAttachmentTask =
                this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() => removeStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageStudentAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment(randomDateTime);
            Guid inputAttachmentId = randomStudentAttachment.AttachmentId;
            Guid inputStudentId = randomStudentAttachment.StudentId;
            StudentAttachment nullStorageStudentAttachment = null;

            var notFoundStudentAttachmentException =
                new NotFoundStudentAttachmentException(inputStudentId, inputAttachmentId);

            var expectedSemesterCourseValidationException =
                new StudentAttachmentValidationException(notFoundStudentAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId))
                    .ReturnsAsync(nullStorageStudentAttachment);

            // when
            ValueTask<StudentAttachment> removeStudentAttachmentTask =
                this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() =>
                removeStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}