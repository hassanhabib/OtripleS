// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Registrations;
using OtripleS.Web.Api.Tests.Acceptance.Models.Users;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Registrations
{
    [Collection(nameof(ApiTestCollection))]
    public partial class RegistrationsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public RegistrationsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async ValueTask<Registration> DeleteRegistrationAsync(Registration registration)
        {
            Registration deletedRegistration =
                await this.otripleSApiBroker.DeleteRegistrationByIdAsync(registration.Id);

            await this.otripleSApiBroker.DeleteUserByIdAsync(deletedRegistration.CreatedBy);

            return deletedRegistration;
        }

        private static Registration CreateRandomRegistration(User user) =>
            CreateRandomRegistrationFiller(user).Create();

        private async ValueTask<Registration> CreateRandomRegistrationAsync()
        {
            User user = await PostRandomUserAsync();

            return CreateRandomRegistrationFiller(user).Create();
        }

        private async ValueTask<Registration> PostRandomRegistrationAsync()
        {
            User user = await PostRandomUserAsync();
            Registration randomRegistration = CreateRandomRegistration(user);
            await this.otripleSApiBroker.PostRegistrationAsync(randomRegistration);

            return randomRegistration;
        }

        private async ValueTask<User> PostRandomUserAsync()
        {
            User user = CreateRandomUser();

            return await this.otripleSApiBroker.PostUserAsync(user);
        }

        private static User CreateRandomUser() =>
            CreateRandomUserFiller().Create();

        private static Registration UpdateRegistrationRandom(Registration registration)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            registration.UpdatedDate = now;
            registration.SubmitterName = GetRandomString();

            return registration;
        }

        private static string GetRandomString() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 10).GetValue();

        private static string GetRandomEmailAddress() => new EmailAddresses().GetValue();

        private static string GetRandomPhoneNumber() =>
            new PatternGenerator("{N:3}-{N:3}-{N:4}").GetValue();

        private static Filler<User> CreateRandomUserFiller()
        {
            var now = DateTimeOffset.UtcNow;
            DateTimeOffset? nullableDateTime = null;
            var filler = new Filler<User>();

            filler.Setup()
                .OnProperty(user => user.CreatedDate).Use(now)
                .OnProperty(user => user.UpdatedDate).Use(now)
                .OnProperty(user => user.LockoutEnd).Use(nullableDateTime)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Registration> CreateRandomRegistrationFiller(User user)
        {
            var now = DateTimeOffset.UtcNow;
            var filler = new Filler<Registration>();
            var email = GetRandomEmailAddress();

            filler.Setup()
                .OnProperty(registration => registration.CreatedBy).Use(user.Id)
                .OnProperty(registration => registration.UpdatedBy).Use(user.Id)
                .OnProperty(registration => registration.CreatedDate).Use(now)
                .OnProperty(registration => registration.UpdatedDate).Use(now)
                .OnProperty(registration => registration.StudentEmail).Use(email)
                .OnProperty(registration => registration.StudentPhone).Use(GetRandomPhoneNumber)
                .OnProperty(registration => registration.SubmitterEmail).Use(email)
                .OnProperty(registration => registration.SubmitterPhone).Use(GetRandomPhoneNumber)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
