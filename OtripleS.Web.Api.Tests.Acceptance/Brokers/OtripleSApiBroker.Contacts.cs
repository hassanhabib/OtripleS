using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string ContactsRelativeUrl = "api/contacts";

        public async ValueTask<Contact> PostContactAsync(Contact contact) =>
            await this.apiFactoryClient.PostContentAsync(ContactsRelativeUrl, contact);

        public async ValueTask<Contact> GetContactByIdAsync(Guid contactId) =>
            await this.apiFactoryClient.GetContentAsync<Contact>($"{ContactsRelativeUrl}/{contactId}");

        public async ValueTask<Contact> DeleteContactByIdAsync(Guid contactId) =>
            await this.apiFactoryClient.DeleteContentAsync<Contact>($"{ContactsRelativeUrl}/{contactId}");

        public async ValueTask<Contact> PutContactAsync(Contact contact) =>
            await this.apiFactoryClient.PutContentAsync(ContactsRelativeUrl, contact);

        public async ValueTask<List<Contact>> GetAllContactsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Contact>>($"{ContactsRelativeUrl}/");
    }
}