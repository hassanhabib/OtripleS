// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.GuardianContacts;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string GuardianContactsRelativeUrl = "api/GuardianContacts";

        public async ValueTask<GuardianContact> PostGuardianContactAsync(GuardianContact guardian) =>
            await this.apiFactoryClient.PostContentAsync(GuardianContactsRelativeUrl, guardian);

        public async ValueTask<GuardianContact> GetGuardianContactByIdAsync(Guid guardianId, Guid contactId) =>
            await this.apiFactoryClient.GetContentAsync<GuardianContact>(
                $"{GuardianContactsRelativeUrl}/guardians/{guardianId}/contacts/{contactId}");

        public async ValueTask<GuardianContact> DeleteGuardianContactByIdAsync(Guid guardianId, Guid contactId) =>
            await this.apiFactoryClient.DeleteContentAsync<GuardianContact>(
                $"{GuardianContactsRelativeUrl}/guardians/{guardianId}/contacts/{contactId}");

        public async ValueTask<List<GuardianContact>> GetAllGuardianContactsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<GuardianContact>>($"{GuardianContactsRelativeUrl}/");
    }
}
