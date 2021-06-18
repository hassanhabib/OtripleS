//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Foundations.GuardianAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenGuardianAttachmentsWereEmptyAndLogIt()
        {
            // given
            IQueryable<GuardianAttachment> emptyStorageGuardianAttachments = new List<GuardianAttachment>().AsQueryable();
            IQueryable<GuardianAttachment> expectedGuardianAttachments = emptyStorageGuardianAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardianAttachments())
                    .Returns(expectedGuardianAttachments);

            // when
            IQueryable<GuardianAttachment> actualGuardianAttachments =
                this.guardianAttachmentService.RetrieveAllGuardianAttachments();

            // then
            actualGuardianAttachments.Should().BeEquivalentTo(emptyStorageGuardianAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardianAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No guardian attachments found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
