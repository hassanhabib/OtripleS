// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAttachmentAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();

            Attachment randomAttachment =
                CreateRandomAttachment(dates: dateTime);

            Guid inputAttachmentId = randomAttachment.Id;
            Attachment inputAttachment = randomAttachment;
            Attachment storageAttachment = inputAttachment;
            Attachment expectedAttachment = storageAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId))
                    .ReturnsAsync(inputAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAttachmentAsync(inputAttachment))
                    .ReturnsAsync(storageAttachment);

            // when
            Attachment actualAttachment =
                await this.attachmentService.RemoveAttachmentByIdAsync(inputAttachmentId);

            // then
            actualAttachment.Should().BeEquivalentTo(expectedAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttachmentAsync(inputAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
