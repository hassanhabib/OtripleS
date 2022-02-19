// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Fees;
using OtripleS.Web.Api.Tests.Acceptance.Models.Users;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Fees
{
    [Collection(nameof(ApiTestCollection))]
    public partial class FeesApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public FeesApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async ValueTask<Fee> DeleteFeeAsync(Fee fee)
        {
            Fee deletedFee =
                await this.otripleSApiBroker.DeleteFeeByIdAsync(fee.Id);

            await this.otripleSApiBroker.DeleteUserByIdAsync(fee.CreatedBy);

            return deletedFee;
        }

        private async ValueTask<Fee> PostRandomFeeAsync()
        {
            Fee fee = await CreateRandomFeeAsync();

            return await this.otripleSApiBroker.PostFeeAsync(fee);
        }

        private async ValueTask<User> PostRandomUserAsync()
        {
            User user = CreateRandomUser();

            return await this.otripleSApiBroker.PostUserAsync(user);
        }

        private static User CreateRandomUser() =>
            CreateRandomUserFiller().Create();

        private async ValueTask<Fee> CreateRandomFeeAsync() =>
            await CreateRandomFeeFiller();

        private static Fee UpdateFeeRandom(Fee fee)
        {
            fee.Label = GetRandomString();
            fee.UpdatedDate = DateTimeOffset.UtcNow;

            return fee;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() => new MnemonicString().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<Fee> CreateRandomFeeFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            User user = await PostRandomUserAsync();

            var filler = new Filler<Fee>();

            filler.Setup()
                .OnProperty(fee => fee.CreatedBy).Use(user.Id)
                .OnProperty(fee => fee.UpdatedBy).Use(user.Id)
                .OnProperty(fee => fee.CreatedDate).Use(now)
                .OnProperty(fee => fee.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Filler<User> CreateRandomUserFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            DateTimeOffset? nullableDateTime = null;
            var filler = new Filler<User>();

            filler.Setup()
                .OnProperty(user => user.CreatedDate).Use(now)
                .OnProperty(user => user.UpdatedDate).Use(now)
                .OnProperty(user => user.LockoutEnd).Use(nullableDateTime)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
