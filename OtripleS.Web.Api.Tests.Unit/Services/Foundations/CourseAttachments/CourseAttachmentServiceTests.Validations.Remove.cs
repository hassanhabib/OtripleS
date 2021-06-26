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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenCourseIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomCourseId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid invalidCourseId = randomCourseId;

            var invalidCourseAttachmentInputException = new InvalidCourseAttachmentException(
                parameterName: nameof(CourseAttachment.CourseId),
                parameterValue: invalidCourseId);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(invalidCourseId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
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
            Guid randomCourseId = Guid.NewGuid();
            Guid invalidAttachmentId = randomAttachmentId;
            Guid inputCourseId = randomCourseId;

            var invalidCourseAttachmentInputException = new InvalidCourseAttachmentException(
                parameterName: nameof(CourseAttachment.AttachmentId),
                parameterValue: invalidAttachmentId);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(inputCourseId, invalidAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageCourseAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment(randomDateTime);
            Guid someAttachmentId = randomCourseAttachment.AttachmentId;
            Guid someCourseId = randomCourseAttachment.CourseId;
            CourseAttachment nullStorageCourseAttachment = null;

            var notFoundCourseAttachmentException =
                new NotFoundCourseAttachmentException(someCourseId, someAttachmentId);

            var expectedCourseValidationException =
                new CourseAttachmentValidationException(notFoundCourseAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId))
                    .ReturnsAsync(nullStorageCourseAttachment);

            // when
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(someCourseId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
