// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.AssignmentsAttachments;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.AssignmentsAttachments
{
    public partial class AssignmentAttachmentsApiTests
    {
        [Fact]
        public async Task ShouldPostAssignmentAttachmentAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = await CreateRandomAssignmentAttachment();
            AssignmentAttachment inputAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = inputAssignmentAttachment;

            // when             
            AssignmentAttachment actualAssignmentAttachment =
                await this.otripleSApiBroker.PostAssignmentAttachmentAsync(inputAssignmentAttachment);

            AssignmentAttachment retrievedAssignmentAttachment =
                await this.otripleSApiBroker.GetAssignmentAttachmentByIdsAsync(
                    inputAssignmentAttachment.AssignmentId,
                    inputAssignmentAttachment.AttachmentId);

            // then
            actualAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);
            retrievedAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);
            await DeleteAssignmentAttachmentAsync(actualAssignmentAttachment);
        }

        [Fact]
        public async Task ShouldGetAllAssignmentAttachmentsAsync()
        {
            // given
            var randomAssignmentAttachments = new List<AssignmentAttachment>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                AssignmentAttachment randomAssignmentAttachment = await PostAssignmentAttachmentAsync();
                randomAssignmentAttachments.Add(randomAssignmentAttachment);
            }

            List<AssignmentAttachment> inputAssignmentAttachments = randomAssignmentAttachments;
            List<AssignmentAttachment> expectedAssignmentAttachments = inputAssignmentAttachments;

            // when
            List<AssignmentAttachment> actualAssignmentAttachments =
                await this.otripleSApiBroker.GetAllAssignmentAttachmentsAsync();

            // then
            foreach (AssignmentAttachment expectedAssignmentAttachment in expectedAssignmentAttachments)
            {
                AssignmentAttachment actualAssignmentAttachment =
                    actualAssignmentAttachments.Single(studentAttachment =>
                        studentAttachment.AssignmentId == expectedAssignmentAttachment.AssignmentId);

                actualAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

                await DeleteAssignmentAttachmentAsync(actualAssignmentAttachment);
            }
        }

        [Fact]
        public async Task ShouldDeleteAssignmentAttachmentAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = await PostAssignmentAttachmentAsync();
            AssignmentAttachment inputAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = inputAssignmentAttachment;

            // when 
            AssignmentAttachment deletedAssignmentAttachment =
                await DeleteAssignmentAttachmentAsync(inputAssignmentAttachment);

            ValueTask<AssignmentAttachment> getAssignmentAttachmentByIdTask =
                this.otripleSApiBroker.GetAssignmentAttachmentByIdsAsync(
                    inputAssignmentAttachment.AssignmentId,
                    inputAssignmentAttachment.AttachmentId);

            // then
            deletedAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getAssignmentAttachmentByIdTask.AsTask());
        }
    }
}
