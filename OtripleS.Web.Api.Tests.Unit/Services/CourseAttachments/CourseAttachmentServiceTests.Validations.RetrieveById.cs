//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenCourseIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomCourseId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCourseId = randomCourseId;

            var invalidCourseAttachmentInputException = new InvalidCourseAttachmentException(
                parameterName: nameof(CourseAttachment.CourseId),
                parameterValue: inputCourseId);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> retrieveCourseAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync(
                    inputCourseId, 
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() => 
                retrieveCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid randomCourseId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCourseId = randomCourseId;

            var invalidCourseAttachmentInputException = new InvalidCourseAttachmentException(
                parameterName: nameof(CourseAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> retrieveCourseAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync(
                    inputCourseId, 
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                retrieveCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageCourseAttachmentIsInvalidAndLogItAsync()
        {
            // given
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment();
            Guid inputAttachmentId = randomCourseAttachment.AttachmentId;
            Guid inputCourseId = randomCourseAttachment.CourseId;
            CourseAttachment nullStorageCourseAttachment = null;

            var notFoundCourseAttachmentException =
                new NotFoundCourseAttachmentException(inputCourseId, inputAttachmentId);

            var expectedAttachmentValidationException =
                new CourseAttachmentValidationException(notFoundCourseAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCourseAttachmentByIdAsync(inputCourseId, inputAttachmentId))
                    .ReturnsAsync(nullStorageCourseAttachment);

            // when
            ValueTask<CourseAttachment> retrieveCourseAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync(
                    inputCourseId, 
                    inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                retrieveCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
