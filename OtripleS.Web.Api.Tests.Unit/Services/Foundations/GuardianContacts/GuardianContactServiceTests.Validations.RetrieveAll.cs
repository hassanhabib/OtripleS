//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Foundations.GuardianContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenGuardianContactsWereEmptyAndLogIt()
        {
            // given
            IQueryable<GuardianContact> emptyStorageGuardianContacts =
                new List<GuardianContact>().AsQueryable();

            IQueryable<GuardianContact> expectedGuardianContacts =
                emptyStorageGuardianContacts;

            string expectedMessage = "No GuardianContacts found in storage.";

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardianContacts())
                    .Returns(expectedGuardianContacts);

            // when
            IQueryable<GuardianContact> actualSemesterCourses =
                this.guardianContactService.RetrieveAllGuardianContacts();

            // then
            actualSemesterCourses.Should().BeEquivalentTo(emptyStorageGuardianContacts);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning(expectedMessage),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardianContacts(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
