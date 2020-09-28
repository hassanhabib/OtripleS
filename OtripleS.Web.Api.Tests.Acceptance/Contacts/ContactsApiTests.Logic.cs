using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Contacts;
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
    }
}