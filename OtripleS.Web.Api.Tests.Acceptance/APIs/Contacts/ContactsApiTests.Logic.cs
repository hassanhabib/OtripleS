// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.Contacts
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
            Contact randomContact = CreateRandomContact();
            await this.otripleSApiBroker.PostContactAsync(randomContact);
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
            IEnumerable<Contact> randomContacts = CreateRandomContacts();
            IEnumerable<Contact> inputContacts = randomContacts;

            foreach (Contact contact in inputContacts)
            {
                await this.otripleSApiBroker.PostContactAsync(contact);
            }

            List<Contact> expectedContacts = inputContacts.ToList();

            // when
            List<Contact> actualContacts =
                await this.otripleSApiBroker.GetAllContactsAsync();

            // then
            foreach (Contact expectedContact in expectedContacts)
            {
                Contact actualContact = actualContacts.Single(contact => contact.Id == expectedContact.Id);
                actualContact.Should().BeEquivalentTo(expectedContact);
                await this.otripleSApiBroker.DeleteContactByIdAsync(actualContact.Id);
            }
        }
    }
}