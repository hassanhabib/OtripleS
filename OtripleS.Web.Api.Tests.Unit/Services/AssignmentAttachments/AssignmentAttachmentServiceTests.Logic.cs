//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAssignmentAttachmentAsync()
        {
            // given
            var randomAssignmentId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputAssignmentId = randomAssignmentId;
            Guid inputAttachmentId = randomAttachmentId;
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            randomAssignmentAttachment.AssignmentId = inputAssignmentId;
            randomAssignmentAttachment.AttachmentId = inputAttachmentId;
            AssignmentAttachment storageAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = storageAssignmentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(inputAssignmentId, inputAttachmentId))
                    .ReturnsAsync(storageAssignmentAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAssignmentAttachmentAsync(storageAssignmentAttachment))
                    .ReturnsAsync(expectedAssignmentAttachment);

            // when
            AssignmentAttachment actualAssignmentAttachment =
                await this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(inputAssignmentId, inputAttachmentId);

            // then
            actualAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(inputAssignmentId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(storageAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
