// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomAttachmentId = default;
            Guid inputAttachmentId = randomAttachmentId;

            var invalidAttachmentInputException = new InvalidAttachmentException(
                parameterName: nameof(Attachment.Id),
                parameterValue: inputAttachmentId);

            var expectedAttachmentValidationException = new AttachmentValidationException(invalidAttachmentInputException);

            //when
            ValueTask<Attachment> retrieveAttachmentByIdTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(inputAttachmentId);

            //then
            await Assert.ThrowsAsync<AttachmentValidationException>(() => retrieveAttachmentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageAttachmentIsNullAndLogItAsync()
        {
            //given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Attachment invalidStorageAttachment = null;

            var notFoundAttachmentException = new NotFoundAttachmentException(inputAttachmentId);

            var expectedAttachmentValidationException = new AttachmentValidationException(notFoundAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAttachmentByIdAsync(inputAttachmentId))
                .ReturnsAsync(invalidStorageAttachment);

            //when
            ValueTask<Attachment> retrieveAttachmentByIdTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(inputAttachmentId);

            //then
            await Assert.ThrowsAsync<AttachmentValidationException>(() =>
                retrieveAttachmentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
