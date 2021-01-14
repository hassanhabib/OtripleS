//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.GuardianAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllGuardianAttachments()
        {
            // given
            IQueryable<GuardianAttachment> randomGuardianAttachments = CreateRandomGuardianAttachments();
            IQueryable<GuardianAttachment> storageGuardianAttachments = randomGuardianAttachments;
            IQueryable<GuardianAttachment> expectedGuardianAttachments = storageGuardianAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardianAttachments())
                    .Returns(storageGuardianAttachments);

            // when
            IQueryable<GuardianAttachment> actualGuardianAttachments =
                this.guardianAttachmentService.RetrieveAllGuardianAttachments();

            // then
            actualGuardianAttachments.Should().BeEquivalentTo(expectedGuardianAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardianAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveGuardianAttachmentById()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment storageGuardianAttachment = randomGuardianAttachment;
            GuardianAttachment expectedGuardianAttachment = storageGuardianAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync
                (randomGuardianAttachment.GuardianId, randomGuardianAttachment.AttachmentId))
                    .Returns(new ValueTask<GuardianAttachment>(randomGuardianAttachment));

            // when
            GuardianAttachment actualGuardianAttachment = await
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync(
                    randomGuardianAttachment.GuardianId, randomGuardianAttachment.AttachmentId);

            // then
            actualGuardianAttachment.Should().BeEquivalentTo(expectedGuardianAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync
                (randomGuardianAttachment.GuardianId, randomGuardianAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
