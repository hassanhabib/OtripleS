// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldRemoveGuardianAttachmentAsync()
        {
            // given
            var randomGuardianId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            Guid inputAttachmentId = randomAttachmentId;
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
