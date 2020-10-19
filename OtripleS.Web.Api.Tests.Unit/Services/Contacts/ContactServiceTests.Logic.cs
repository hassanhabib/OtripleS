// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
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

        [Fact]
        public async Task ShouldRetrieveContactById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Guid inputContactId = randomContact.Id;
            Contact inputContact = randomContact;
            Contact expectedContact = randomContact;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectContactByIdAsync(inputContactId))
                .ReturnsAsync(inputContact);

            //when 
            Contact actualContact = await this.contactService.RetrieveContactByIdAsync(inputContactId);

            //then
            actualContact.Should().BeEquivalentTo(expectedContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyContactAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(randomInputDate);
            Contact inputContact = randomContact;
            Contact afterUpdateStorageContact = inputContact;
            Contact expectedContact = afterUpdateStorageContact;
            Contact beforeUpdateStorageContact = randomContact.DeepClone();
            inputContact.UpdatedDate = randomDate;
            Guid contactId = inputContact.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(contactId))
                    .ReturnsAsync(beforeUpdateStorageContact);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateContactAsync(inputContact))
                    .ReturnsAsync(afterUpdateStorageContact);

            // when
            Contact actualContact =
                await this.contactService.ModifyContactAsync(inputContact);

            // then
            actualContact.Should().BeEquivalentTo(expectedContact);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(contactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateContactAsync(inputContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteContactByIdAsync()
        {
            // given
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            Guid inputContactId = inputContact.Id;
            inputContact.UpdatedBy = inputContact.CreatedBy;
            inputContact.UpdatedDate = inputContact.CreatedDate;
            Contact storageContact = inputContact;
            Contact expectedContact = inputContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(inputContactId))
                    .ReturnsAsync(inputContact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteContactAsync(inputContact))
                    .ReturnsAsync(storageContact);

            // when
            Contact actualContact =
                await this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            actualContact.Should().BeEquivalentTo(expectedContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(inputContact),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
