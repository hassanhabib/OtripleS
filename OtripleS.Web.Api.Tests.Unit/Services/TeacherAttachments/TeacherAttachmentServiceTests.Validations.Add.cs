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

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenTeacherAttachmentIsNullAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = default;
            TeacherAttachment nullTeacherAttachment = randomTeacherAttachment;
            var nullTeacherAttachmentException = new NullTeacherAttachmentException();

            var expectedTeacherAttachmentValidationException =
                new TeacherAttachmentValidationException(nullTeacherAttachmentException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(nullTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentValidationException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
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

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
