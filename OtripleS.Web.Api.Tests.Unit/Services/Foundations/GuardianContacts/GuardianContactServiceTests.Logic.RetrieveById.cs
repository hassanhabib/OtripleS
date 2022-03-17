// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveGuardianContactById()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact storageGuardianContact = randomGuardianContact;
            GuardianContact expectedGuardianContact = storageGuardianContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(randomGuardianContact.GuardianId, randomGuardianContact.ContactId))
                    .ReturnsAsync(randomGuardianContact);

            // when
            GuardianContact actualGuardianContact = await
                this.guardianContactService.RetrieveGuardianContactByIdAsync(
                    randomGuardianContact.GuardianId, randomGuardianContact.ContactId);

            // then
            actualGuardianContact.Should().BeEquivalentTo(expectedGuardianContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(randomGuardianContact.GuardianId, randomGuardianContact.ContactId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}