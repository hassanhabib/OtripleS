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
        public async void ShouldThrowValidationExceptionOnCreateWhenAttachmentIsNullAndLogItAsync()
        {
            // given
            Attachment randomAttachment = null;
            Attachment nullAttachment = randomAttachment;
            var nullAttachmentException = new NullAttachmentException();

            var expectedAttachmentValidationException =
                new AttachmentValidationException(nullAttachmentException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.InsertAttachmentAsync(nullAttachment);

            // then
            await Assert.ThrowsAsync<AttachmentValidationException>(() =>
                createAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
