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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = default;
            Guid inputAttachmentId = randomAttachmentId;

            var invalidAttachmentException = new InvalidAttachmentException(
                parameterName: nameof(Attachment.Id),
                parameterValue: inputAttachmentId);

            var expectedAttachmentValidationException =
                new AttachmentValidationException(invalidAttachmentException);

            // when
            ValueTask<Attachment> actualAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(inputAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentValidationException>(() => actualAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStorageAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dates: dateTimeOffset);
            Guid inputAttachmentId = randomAttachment.Id;
            Attachment inputAttachment = randomAttachment;
            Attachment nullStorageAttachment = null;

            var notFoundAttachmentException = new NotFoundAttachmentException(inputAttachmentId);

            var expectedAttachmentValidationException =
                new AttachmentValidationException(notFoundAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId))
                    .ReturnsAsync(nullStorageAttachment);

            // when
            ValueTask<Attachment> actualAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(inputAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentValidationException>(() => actualAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttachmentAsync(It.IsAny<Attachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
