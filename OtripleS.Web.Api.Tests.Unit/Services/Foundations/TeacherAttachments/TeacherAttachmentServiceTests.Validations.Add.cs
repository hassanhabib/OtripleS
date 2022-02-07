//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenTeacherAttachmentIsNullAndLogItAsync()
        {
            // given
            TeacherAttachment invalidTeacherAttachment = null;
            
            var nullTeacherAttachmentException = new NullTeacherAttachmentException();

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(nullTeacherAttachmentException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(invalidTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenTeacherIdIsInvalidAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment inputTeacherAttachment = randomTeacherAttachment;
            inputTeacherAttachment.TeacherId = default;

            var invalidTeacherAttachmentInputException = new InvalidTeacherAttachmentException(
                parameterName: nameof(TeacherAttachment.TeacherId),
                parameterValue: inputTeacherAttachment.TeacherId);

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(invalidTeacherAttachmentInputException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(inputTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAttachmentIdIsInvalidAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment inputTeacherAttachment = randomTeacherAttachment;
            inputTeacherAttachment.AttachmentId = default;

            var invalidTeacherAttachmentInputException = new InvalidTeacherAttachmentException(
                parameterName: nameof(TeacherAttachment.AttachmentId),
                parameterValue: inputTeacherAttachment.AttachmentId);

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(invalidTeacherAttachmentInputException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(inputTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenTeacherAttachmentAlreadyExistsAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment alreadyExistsTeacherAttachment = randomTeacherAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsTeacherAttachmentException =
                new AlreadyExistsTeacherAttachmentException(duplicateKeyException);

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(alreadyExistsTeacherAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAttachmentAsync(alreadyExistsTeacherAttachment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(alreadyExistsTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(alreadyExistsTeacherAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment invalidTeacherAttachment = randomTeacherAttachment;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidTeacherAttachmentReferenceException =
                new InvalidTeacherAttachmentReferenceException(foreignKeyConstraintConflictException);

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(invalidTeacherAttachmentReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAttachmentAsync(invalidTeacherAttachment))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(invalidTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(invalidTeacherAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
