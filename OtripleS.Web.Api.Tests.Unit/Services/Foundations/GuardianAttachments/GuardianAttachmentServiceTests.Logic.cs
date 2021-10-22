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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {

        [Fact]
        public async Task ShouldAddGuardianAttachmentAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment inputGuardianAttachment = randomGuardianAttachment;
            GuardianAttachment storageGuardianAttachment = randomGuardianAttachment;
            GuardianAttachment expectedGuardianAttachment = storageGuardianAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAttachmentAsync(inputGuardianAttachment))
                    .ReturnsAsync(storageGuardianAttachment);

            // when
            GuardianAttachment actualGuardianAttachment =
                await this.guardianAttachmentService.AddGuardianAttachmentAsync(inputGuardianAttachment);

            // then
            actualGuardianAttachment.Should().BeEquivalentTo(expectedGuardianAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(inputGuardianAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

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
                    .ReturnsAsync(storageGuardianAttachment);

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

        [Fact]
        public async Task ShouldRemoveGuardianAttachmentAsync()
        {
            // given
            Guid inputGuardianId = Guid.NewGuid();
            Guid inputAttachmentId = Guid.NewGuid();
            DateTimeOffset inputDateTime = GetRandomDateTime();
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment(inputDateTime);
            randomGuardianAttachment.GuardianId = inputGuardianId;
            randomGuardianAttachment.AttachmentId = inputAttachmentId;
            GuardianAttachment storageGuardianAttachment = randomGuardianAttachment;
            GuardianAttachment expectedGuardianAttachment = storageGuardianAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId))
                    .ReturnsAsync(storageGuardianAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteGuardianAttachmentAsync(storageGuardianAttachment))
                    .ReturnsAsync(expectedGuardianAttachment);

            // when
            GuardianAttachment actualGuardianAttachment =
                await this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync(
                    inputGuardianId, inputAttachmentId);

            // then
            actualGuardianAttachment.Should().BeEquivalentTo(expectedGuardianAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAttachmentAsync(storageGuardianAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
