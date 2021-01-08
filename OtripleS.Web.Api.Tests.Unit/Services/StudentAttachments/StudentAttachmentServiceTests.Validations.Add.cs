//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentAttachmentIsNullAndLogItAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = default;
            StudentAttachment nullStudentAttachment = randomStudentAttachment;
            var nullStudentAttachmentException = new NullStudentAttachmentException();

            var expectedStudentAttachmentValidationException =
                new StudentAttachmentValidationException(nullStudentAttachmentException);

            // when
            ValueTask<StudentAttachment> addStudentAttachmentTask =
                this.studentAttachmentService.AddStudentAttachmentAsync(nullStudentAttachment);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() =>
                addStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment inputStudentAttachment = randomStudentAttachment;
            inputStudentAttachment.StudentId = default;

            var invalidStudentAttachmentInputException = new InvalidStudentAttachmentException(
                parameterName: nameof(StudentAttachment.StudentId),
                parameterValue: inputStudentAttachment.StudentId);

            var expectedStudentAttachmentValidationException =
                new StudentAttachmentValidationException(invalidStudentAttachmentInputException);

            // when
            ValueTask<StudentAttachment> addStudentAttachmentTask =
                this.studentAttachmentService.AddStudentAttachmentAsync(inputStudentAttachment);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() =>
                addStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment inputStudentAttachment = randomStudentAttachment;
            inputStudentAttachment.AttachmentId = default;

            var invalidStudentAttachmentInputException = new InvalidStudentAttachmentException(
                parameterName: nameof(StudentAttachment.AttachmentId),
                parameterValue: inputStudentAttachment.AttachmentId);

            var expectedStudentAttachmentValidationException =
                new StudentAttachmentValidationException(invalidStudentAttachmentInputException);

            // when
            ValueTask<StudentAttachment> addStudentAttachmentTask =
                this.studentAttachmentService.AddStudentAttachmentAsync(inputStudentAttachment);

            // then
            await Assert.ThrowsAsync<StudentAttachmentValidationException>(() =>
                addStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
