﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCourseAttachmentIsNullAndLogItAsync()
        {
            // given
            CourseAttachment invalidCourseAttachment = null;
            var nullCourseAttachmentException = new NullCourseAttachmentException();

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(nullCourseAttachmentException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(invalidCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseAttachmentValidationException))),
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

            var invalidCourseAttachmentInputException = new InvalidCourseAttachmentException();

            invalidCourseAttachmentInputException.AddData(
                key: nameof(CourseAttachment.CourseId),
                values: "Id is required");

            invalidCourseAttachmentInputException.AddData(
                key: nameof(CourseAttachment.AttachmentId),
                values: "Id is required");

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentInputException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(inputCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseAttachmentValidationException))),
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
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCourseAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment();
            CourseAttachment someCourseAttachment = randomCourseAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidCourseAttachmentReferenceException =
                new InvalidCourseAttachmentReferenceException(foreignKeyConstraintConflictException);

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(invalidCourseAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAttachmentAsync(someCourseAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(someCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCourseAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
