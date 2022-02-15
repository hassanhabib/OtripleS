﻿//---------------------------------------------------------------
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
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenIdsAreInvalidAndLogItAsync()
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
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(
                    invalidCourseId,
                    invalidAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCourseAttachmentValidationException))),
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
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageCourseAttachmentIsNotFoundAndLogItAsync()
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))),
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
