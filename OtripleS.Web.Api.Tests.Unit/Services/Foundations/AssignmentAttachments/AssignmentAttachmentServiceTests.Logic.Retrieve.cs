// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAssignmentAttachmentByIdAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment storageAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = storageAssignmentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(
                    randomAssignmentAttachment.AssignmentId,
                    randomAssignmentAttachment.AttachmentId))
                        .ReturnsAsync(randomAssignmentAttachment);

            // when
            AssignmentAttachment actualAssignmentAttachment = await
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    randomAssignmentAttachment.AssignmentId,
                    randomAssignmentAttachment.AttachmentId);

            // then
            actualAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(
                    randomAssignmentAttachment.AssignmentId,
                    randomAssignmentAttachment.AttachmentId),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
