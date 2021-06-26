// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Guardians;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string GuardiansRelativeUrl = "api/guardians";

        public async ValueTask<Guardian> PostGuardianAsync(Guardian guardian) =>
            await this.apiFactoryClient.PostContentAsync(GuardiansRelativeUrl, guardian);

        public async ValueTask<Guardian> GetGuardianByIdAsync(Guid guardianId) =>
            await this.apiFactoryClient.GetContentAsync<Guardian>($"{GuardiansRelativeUrl}/{guardianId}");

        public async ValueTask<Guardian> DeleteGuardianByIdAsync(Guid guardianId) =>
            await this.apiFactoryClient.DeleteContentAsync<Guardian>($"{GuardiansRelativeUrl}/{guardianId}");

        public async ValueTask<Guardian> PutGuardianAsync(Guardian guardian) =>
            await this.apiFactoryClient.PutContentAsync(GuardiansRelativeUrl, guardian);

        public async ValueTask<List<Guardian>> GetAllGuardiansAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Guardian>>($"{GuardiansRelativeUrl}/");
    }
}
