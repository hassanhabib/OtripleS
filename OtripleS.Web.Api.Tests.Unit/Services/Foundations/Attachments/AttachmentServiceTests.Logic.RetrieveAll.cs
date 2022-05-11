// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {

        [Fact]
        public void ShouldRetrieveAllAttachments()
        {
            // given
            IQueryable<Attachment> randomAttachments = CreateRandomAttachments();
            IQueryable<Attachment> storageAttachments = randomAttachments;
            IQueryable<Attachment> expectedAttachments = storageAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttachments())
                    .Returns(storageAttachments);

            // when
            IQueryable<Attachment> actualAttachments =
                this.attachmentService.RetrieveAllAttachments();

            // then
            actualAttachments.Should().BeEquivalentTo(expectedAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
