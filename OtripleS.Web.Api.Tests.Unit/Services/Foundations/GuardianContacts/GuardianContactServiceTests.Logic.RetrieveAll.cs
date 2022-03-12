// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllGuardianContacts()
        {
            // given
            IQueryable<GuardianContact> randomGuardianContacts =
                CreateRandomGuardianContacts();

            IQueryable<GuardianContact> storageGuardianContacts = randomGuardianContacts;
            IQueryable<GuardianContact> expectedGuardianContacts = storageGuardianContacts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardianContacts())
                    .Returns(storageGuardianContacts);

            // when
            IQueryable<GuardianContact> actualGuardianContacts =
                this.guardianContactService.RetrieveAllGuardianContacts();

            // then
            actualGuardianContacts.Should().BeEquivalentTo(expectedGuardianContacts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardianContacts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
