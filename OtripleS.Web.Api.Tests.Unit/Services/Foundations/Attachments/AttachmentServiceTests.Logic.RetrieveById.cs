// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldRetrieveAttachmentByIdAsync()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();

            Attachment randomAttachment =
                CreateRandomAttachment(dateTime);

            Guid inputAttachmentId = randomAttachment.Id;
            Attachment inputAttachment = randomAttachment;
            Attachment expectedAttachment = randomAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId))
                    .ReturnsAsync(inputAttachment);

            //when 
            Attachment actualAttachment =
                await this.attachmentService.RetrieveAttachmentByIdAsync(inputAttachmentId);

            //then
            actualAttachment.Should().BeEquivalentTo(expectedAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
