// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.AssignmentsAttachments;
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

    }
}
