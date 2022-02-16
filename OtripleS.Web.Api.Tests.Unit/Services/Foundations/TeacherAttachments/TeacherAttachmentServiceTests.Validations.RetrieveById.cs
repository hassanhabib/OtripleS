﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenTeacherIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomTeacherId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputTeacherId = randomTeacherId;

            var invalidTeacherAttachmentInputException = new InvalidTeacherAttachmentException(
                parameterName: nameof(TeacherAttachment.TeacherId),
                parameterValue: inputTeacherId);

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(invalidTeacherAttachmentInputException);

            // when
            ValueTask<TeacherAttachment> actualTeacherAttachmentTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(inputTeacherId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() => actualTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputTeacherId = randomTeacherId;

            var invalidTeacherAttachmentInputException = new InvalidTeacherAttachmentException(
                parameterName: nameof(TeacherAttachment.AttachmentId),
                parameterValue: inputAttachmentId);

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(invalidTeacherAttachmentInputException);

            // when
            ValueTask<TeacherAttachment> actualTeacherAttachmentTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(inputTeacherId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() => actualTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageTeacherAttachmentIsInvalidAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            Guid inputAttachmentId = randomTeacherAttachment.AttachmentId;
            Guid inputTeacherId = randomTeacherAttachment.TeacherId;
            TeacherAttachment nullStorageTeacherAttachment = null;

            var notFoundTeacherAttachmentException =
                new NotFoundTeacherAttachmentException(inputTeacherId, inputAttachmentId);

            var expectedAttachmentValidationException =
                new TeacherAttachmentValidationException(notFoundTeacherAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherAttachmentByIdAsync(inputTeacherId, inputAttachmentId))
                    .ReturnsAsync(nullStorageTeacherAttachment);

            // when
            ValueTask<TeacherAttachment> actualTeacherAttachmentRetrieveTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(inputTeacherId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() =>
                actualTeacherAttachmentRetrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
