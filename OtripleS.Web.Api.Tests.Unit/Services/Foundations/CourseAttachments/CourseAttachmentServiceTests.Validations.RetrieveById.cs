// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenIdsAreInvalidAndLogItAsync()
        {
            // given
            Guid invalidCourseId = Guid.Empty;
            Guid invalidAttachmentId = Guid.Empty;

            var invalidCourseAttachmentInputException =
                new InvalidCourseAttachmentException();

            invalidCourseAttachmentInputException.AddData(
                key: nameof(CourseAttachment.CourseId),
                values: "Id is required");

            invalidCourseAttachmentInputException.AddData(
                key: nameof(CourseAttachment.AttachmentId),
                values: "Id is required");

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> retrieveCourseAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync(
                    invalidCourseId,
                    invalidAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                retrieveCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCourseAttachmentValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
