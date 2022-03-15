// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
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
        }


    }
}
