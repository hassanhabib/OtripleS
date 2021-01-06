// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attachments;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Attachments
{
    public partial class AttachmentsApiTests
    {
        [Fact]
        public async Task ShouldGetAllAttachmentsAsync()
        {
            // given
            var randomAttachments = new List<Attachment>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                randomAttachments.Add(await PostRandomAttachmentAsync());
            }

            List<Attachment> inputAttachments = randomAttachments;
            List<Attachment> expectedAttachments = inputAttachments.ToList();

            // when
            List<Attachment> actualAttachments =
                await this.otripleSApiBroker.GetAllAttachmentsAsync();

            // then
            foreach (Attachment expectedAttachment in expectedAttachments)
            {
                Attachment actualAttachment =
                    actualAttachments.Single(attachment =>
                        attachment.Id == expectedAttachment.Id);

                actualAttachment.Should().BeEquivalentTo(expectedAttachment);
                await this.otripleSApiBroker.DeleteAttachmentByIdAsync(actualAttachment.Id);
            }
        }

    }
}