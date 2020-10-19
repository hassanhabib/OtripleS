// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenContactsWasEmptyAndLogIt()
        {
            // given
            IQueryable<Contact> emptyStorageContacts = new List<Contact>().AsQueryable();
            IQueryable<Contact> expectedContacts = emptyStorageContacts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllContacts())
                    .Returns(expectedContacts);

            // when
            IQueryable<Contact> actualContacts =
                this.contactService.RetrieveAllContacts();

            // then
            actualContacts.Should().BeEquivalentTo(emptyStorageContacts);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No contacts found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllContacts(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
