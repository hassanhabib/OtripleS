//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianAttachmentIsNullAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = default;
            GuardianAttachment nullGuardianAttachment = randomGuardianAttachment;
            var nullGuardianAttachmentException = new NullGuardianAttachmentException();

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(nullGuardianAttachmentException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(nullGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment inputGuardianAttachment = randomGuardianAttachment;
            inputGuardianAttachment.GuardianId = default;

            var invalidGuardianAttachmentInputException = new InvalidGuardianAttachmentException(
                parameterName: nameof(GuardianAttachment.GuardianId),
                parameterValue: inputGuardianAttachment.GuardianId);

            var expectedGuardianAttachmentValidationException =
                new GuardianAttachmentValidationException(invalidGuardianAttachmentInputException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(inputGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentValidationException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
