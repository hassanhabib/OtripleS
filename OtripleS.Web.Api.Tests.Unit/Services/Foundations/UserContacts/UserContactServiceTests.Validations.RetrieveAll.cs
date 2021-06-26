//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenUserContactsWereEmptyAndLogIt()
        {
            // given
            IQueryable<UserContact> emptyStorageUserContacts = new List<UserContact>().AsQueryable();
            IQueryable<UserContact> expectedUserContacts = emptyStorageUserContacts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUserContacts())
                    .Returns(expectedUserContacts);

            // when
            IQueryable<UserContact> actualUserContacts =
                this.userContactService.RetrieveAllUserContacts();

            // then
            actualUserContacts.Should().BeEquivalentTo(emptyStorageUserContacts);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No UserContacts found in storage."),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUserContacts(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
