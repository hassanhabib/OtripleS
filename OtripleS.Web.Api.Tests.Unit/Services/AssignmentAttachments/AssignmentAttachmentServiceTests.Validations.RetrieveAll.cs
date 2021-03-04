using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void ShouldLogWarningOnRetrieveAllWhenAssignmentAttachmentsWereEmptyAndLogIt()
        {
            // given
            IQueryable<AssignmentAttachment> emptyStorageAssignmentAttachments =
                new List<AssignmentAttachment>().AsQueryable();

            IQueryable<AssignmentAttachment> storageAssignmentAttachments =
                emptyStorageAssignmentAttachments;

            IQueryable<AssignmentAttachment> expectedAssignmentAttachments =
                emptyStorageAssignmentAttachments;

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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No assignment attachments found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
