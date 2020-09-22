// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Services.Contacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public async Task ShouldAddContactAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            inputContact.UpdatedBy = inputContact.CreatedBy;
            inputContact.UpdatedDate = inputContact.CreatedDate;
            Contact expectedContact = inputContact;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertContactAsync(inputContact))
                    .ReturnsAsync(expectedContact);

            // when
            Contact actualContact = 
                await this.contactService.AddContactAsync(inputContact);

            // then
            actualContact.Should().BeEquivalentTo(expectedContact);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertContactAsync(inputContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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
