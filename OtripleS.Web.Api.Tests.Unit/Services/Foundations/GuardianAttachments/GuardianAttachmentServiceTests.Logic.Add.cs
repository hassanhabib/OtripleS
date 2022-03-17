// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
    }
}
