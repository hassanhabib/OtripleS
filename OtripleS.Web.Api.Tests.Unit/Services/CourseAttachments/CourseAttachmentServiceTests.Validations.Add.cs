using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCourseAttachmentIsNullAndLogItAsync()
        {
            // given
            CourseAttachment nullCourseAttachment = default;
            var nullCourseAttachmentException = new NullCourseAttachmentException();

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(nullCourseAttachmentException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(nullCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCourseIdIsInvalidAndLogItAsync()
        {
            // given
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment();
            CourseAttachment inputCourseAttachment = randomCourseAttachment;
            inputCourseAttachment.CourseId = default;

            var invalidCourseAttachmentInputException = new InvalidCourseAttachmentException(
                parameterName: nameof(CourseAttachment.CourseId),
                parameterValue: inputCourseAttachment.CourseId);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(inputCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment();
            CourseAttachment inputCourseAttachment = randomCourseAttachment;
            inputCourseAttachment.AttachmentId = default;

            var invalidCourseAttachmentInputException = new InvalidCourseAttachmentException(
                parameterName: nameof(CourseAttachment.AttachmentId),
                parameterValue: inputCourseAttachment.AttachmentId);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(inputCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCourseAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment();
            CourseAttachment alreadyExistsCourseAttachment = randomCourseAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCourseAttachmentException =
                new AlreadyExistsCourseAttachmentException(duplicateKeyException);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(alreadyExistsCourseAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAttachmentAsync(alreadyExistsCourseAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(alreadyExistsCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
