// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Fees;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string FeesRelativeUrl = "api/fees";

        public async ValueTask<Fee> PostFeeAsync(Fee fee) =>
            await this.apiFactoryClient.PostContentAsync(FeesRelativeUrl, fee);

        public async ValueTask<Fee> GetFeeByIdAsync(Guid feeId) =>
            await this.apiFactoryClient.GetContentAsync<Fee>($"{FeesRelativeUrl}/{feeId}");

        public async ValueTask<Fee> DeleteFeeByIdAsync(Guid feeId) =>
            await this.apiFactoryClient.DeleteContentAsync<Fee>($"{FeesRelativeUrl}/{feeId}");

        public async ValueTask<Fee> PutFeeAsync(Fee fee) =>
            await this.apiFactoryClient.PutContentAsync(FeesRelativeUrl, fee);

        public async ValueTask<List<Fee>> GetAllFeesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Fee>>($"{FeesRelativeUrl}/");
    }
}
