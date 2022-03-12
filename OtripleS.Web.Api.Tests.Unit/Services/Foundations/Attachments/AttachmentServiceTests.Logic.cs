// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {       
        [Fact]
        public async Task ShouldModifyAttachmentAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();

            Attachment randomAttachment =
                CreateRandomAttachment(randomInputDate);

            Attachment inputAttachment = randomAttachment;
            Attachment afterUpdateStorageAttachment = inputAttachment;
            Attachment expectedAttachment = afterUpdateStorageAttachment;

            Attachment beforeUpdateStorageAttachment =
                randomAttachment.DeepClone();

            inputAttachment.UpdatedDate = randomDate;
            Guid attachmentId = inputAttachment.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(attachmentId))
                    .ReturnsAsync(beforeUpdateStorageAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAttachmentAsync(inputAttachment))
                    .ReturnsAsync(afterUpdateStorageAttachment);

            // when
            Attachment actualAttachment =
                await this.attachmentService.ModifyAttachmentAsync(inputAttachment);

            // then
            actualAttachment.Should().BeEquivalentTo(expectedAttachment);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(attachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAttachmentAsync(inputAttachment),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }        

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
