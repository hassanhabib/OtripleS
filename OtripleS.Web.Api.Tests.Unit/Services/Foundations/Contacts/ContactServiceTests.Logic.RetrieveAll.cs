// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllContacts()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Contact> randomContacts = CreateRandomContacts(randomDateTime);
            IQueryable<Contact> storageContacts = randomContacts;
            IQueryable<Contact> expectedContacts = storageContacts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllContacts())
                    .Returns(storageContacts);

            // when
            IQueryable<Contact> actualContacts =
                this.contactService.RetrieveAllContacts();

            // then
            actualContacts.Should().BeEquivalentTo(expectedContacts);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllContacts(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
