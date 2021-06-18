// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.GuardianContacts;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.GuardianContacts
{
    public partial class GuardianContactsApiTests
    {
        [Fact]
        public async Task ShouldPostGuardianContactAsync()
        {
            // given
            GuardianContact randomGuardianContact = await CreateRandomGuardianContactAsync();
            GuardianContact inputGuardianContact = randomGuardianContact;
            GuardianContact expectedGuardianContact = inputGuardianContact;

            // when 
            await this.otripleSApiBroker.PostGuardianContactAsync(inputGuardianContact);

            GuardianContact actualGuardianContact =
                await this.otripleSApiBroker.GetGuardianContactByIdAsync(
                    inputGuardianContact.GuardianId,
                    inputGuardianContact.ContactId);

            // then
            actualGuardianContact.Should().BeEquivalentTo(expectedGuardianContact);
            await DeleteGuardianContactAsync(actualGuardianContact);
        }

        [Fact]
        public async Task ShouldGetAllGuardianContactsAsync()
        {
            // given
            var randomGuardianContacts = new List<GuardianContact>();

            for (int i = 0; i <= GetRandomNumber(); i++)
            {
                randomGuardianContacts.Add(await PostRandomGuardianContactAsync());
            }

            List<GuardianContact> inputGuardianContacts = randomGuardianContacts;
            List<GuardianContact> expectedGuardianContacts = inputGuardianContacts.ToList();

            // when
            List<GuardianContact> actualGuardianContacts =
                await this.otripleSApiBroker.GetAllGuardianContactsAsync();

            // then
            foreach (GuardianContact expectedGuardianContact in expectedGuardianContacts)
            {
                GuardianContact actualGuardianContact =
                    actualGuardianContacts.Single(guardianContact =>
                        guardianContact.GuardianId == expectedGuardianContact.GuardianId &&
                        guardianContact.ContactId == expectedGuardianContact.ContactId);

                actualGuardianContact.Should().BeEquivalentTo(expectedGuardianContact);
                await DeleteGuardianContactAsync(actualGuardianContact);
            }
        }

        [Fact]
        public async Task ShouldDeleteGuardianContactAsync()
        {
            // given
            GuardianContact randomGuardianContact = await PostRandomGuardianContactAsync();
            GuardianContact inputGuardianContact = randomGuardianContact;
            GuardianContact expectedGuardianContact = inputGuardianContact;

            // when 
            GuardianContact deletedGuardianContact =
                await this.otripleSApiBroker.DeleteGuardianContactByIdAsync(
                    inputGuardianContact.GuardianId,
                    inputGuardianContact.ContactId);

            ValueTask<GuardianContact> getGuardianContactByIdTask =
                this.otripleSApiBroker.GetGuardianContactByIdAsync(
                    inputGuardianContact.GuardianId,
                    inputGuardianContact.ContactId);

            // then
            deletedGuardianContact.Should().BeEquivalentTo(expectedGuardianContact);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getGuardianContactByIdTask.AsTask());
        }
    }
}
