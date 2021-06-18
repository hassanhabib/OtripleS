// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Registrations;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string RegistrationsRelativeUrl = "api/registrations";

        public async ValueTask<Registration> PostRegistrationAsync(Registration registration) =>
            await this.apiFactoryClient.PostContentAsync(RegistrationsRelativeUrl, registration);

        public async ValueTask<Registration> GetRegistrationByIdAsync(Guid registrationId) =>
            await this.apiFactoryClient.GetContentAsync<Registration>($"{RegistrationsRelativeUrl}/{registrationId}");

        public async ValueTask<Registration> DeleteRegistrationByIdAsync(Guid registrationId) =>
            await this.apiFactoryClient.DeleteContentAsync<Registration>($"{RegistrationsRelativeUrl}/{registrationId}");

        public async ValueTask<Registration> PutRegistrationAsync(Registration registration) =>
            await this.apiFactoryClient.PutContentAsync(RegistrationsRelativeUrl, registration);

        public async ValueTask<List<Registration>> GetAllRegistrationsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Registration>>($"{RegistrationsRelativeUrl}/");
    }
}
