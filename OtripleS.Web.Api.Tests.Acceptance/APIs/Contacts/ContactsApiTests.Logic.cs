// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Contacts
{
    public partial class ContactsApiTests
    {
        [Fact]
        public async Task ShouldPostContactAsync()
        {
            // given
            Contact randomContact = CreateRandomContact();
            Contact inputContact = randomContact;
            Contact expectedContact = inputContact;

            // when 
            await this.otripleSApiBroker.PostContactAsync(inputContact);

            Contact actualContact =
                await this.otripleSApiBroker.GetContactByIdAsync(inputContact.Id);

            // then
            actualContact.Should().BeEquivalentTo(expectedContact);
            await this.otripleSApiBroker.DeleteContactByIdAsync(actualContact.Id);
        }

        [Fact]
        public async Task ShouldPutContactAsync()
        {
            // given
            Contact randomContact = await PostRandomContactAsync();
            Contact modifiedContact = UpdateContactRandom(randomContact);

            // when
            await this.otripleSApiBroker.PutContactAsync(modifiedContact);

            Contact actualContact =
                await this.otripleSApiBroker.GetContactByIdAsync(randomContact.Id);

            // then
            actualContact.Should().BeEquivalentTo(modifiedContact);
            await this.otripleSApiBroker.DeleteContactByIdAsync(actualContact.Id);
        }

        [Fact]
        public async Task ShouldGetAllContactsAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            var randomContacts = new List<Contact>();

            for (var i = 0; i <= randomNumber; i++)
            {
                randomContacts.Add(await PostRandomContactAsync());
            }

            List<Contact> inputContacts = randomContacts;
            List<Contact> expectedContacts = inputContacts.ToList();

            // when
            List<Contact> actualContacts =
                await this.otripleSApiBroker.GetAllContactsAsync();

            // then
            foreach (Contact expectedContact in expectedContacts)
            {
                Contact actualContact =
                    actualContacts.Single(contact =>
                        contact.Id == expectedContact.Id);

                actualContact.Should().BeEquivalentTo(expectedContact);
                await this.otripleSApiBroker.DeleteContactByIdAsync(actualContact.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteContactAsync()
        {
            // given
            Contact randomContact = await PostRandomContactAsync();
            Contact inputContact = randomContact;
            Contact expectedContact = inputContact;

            // when 
            Contact deletedContact =
                await this.otripleSApiBroker.DeleteContactByIdAsync(inputContact.Id);

            ValueTask<Contact> getContactByIdTask =
                this.otripleSApiBroker.GetContactByIdAsync(inputContact.Id);

            // then
            deletedContact.Should().BeEquivalentTo(expectedContact);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getContactByIdTask.AsTask());
        }
    }
}