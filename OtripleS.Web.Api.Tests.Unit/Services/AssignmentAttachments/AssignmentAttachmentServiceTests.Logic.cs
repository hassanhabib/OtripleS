//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Linq;
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
        public async Task ShouldAddAttachmentAttachmentAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment inputAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment storageAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = storageAssignmentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAttachmentAsync(inputAssignmentAttachment))
                    .ReturnsAsync(storageAssignmentAttachment);

            // when
            AssignmentAttachment actualAttachmentAttachment =
                await this.assignmentAttachmentService.AddAssignmentAttachmentAsync(inputAssignmentAttachment);

            // then
            actualAttachmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(inputAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllAssignmentAttachments()
        {
            // given
            IQueryable<AssignmentAttachment> randomAssignmentAttachments =
                CreateRandomAssignmentAttachments();

            IQueryable<AssignmentAttachment> storageAssignmentAttachments =
                randomAssignmentAttachments;

            IQueryable<AssignmentAttachment> expectedAssignmentAttachments =
                storageAssignmentAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignmentAttachments())
                    .Returns(storageAssignmentAttachments);

            // when
            IQueryable<AssignmentAttachment> actualAssignmentAttachments =
                this.assignmentAttachmentService.RetrieveAllAssignmentAttachments();

            // then
            actualAssignmentAttachments.Should().BeEquivalentTo(expectedAssignmentAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignmentAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
