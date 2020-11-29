// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Guardians;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.GuardianContacts;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.GuardianContacts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class GuardianContactsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public GuardianContactsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async ValueTask<GuardianContact> PostRandomGuardianContactAsync()
        {
            GuardianContact randomGuardianContact = await CreateRandomGuardianContactAsync();
            await this.otripleSApiBroker.PostGuardianContactAsync(randomGuardianContact);

            return randomGuardianContact;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 10).GetValue();

        private async ValueTask<GuardianContact> CreateRandomGuardianContactAsync()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guardian guardian = await PostRandomGuardianAsync();
            Contact contact = await PostRandomContactAsync();
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<GuardianContact>();

            filler.Setup()
                .OnProperty(guardianContact => guardianContact.GuardianId).Use(guardian.Id)
                .OnProperty(guardianContact => guardianContact.ContactId).Use(contact.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private Guardian CreateRandomGuardian() =>
            CreateRandomGuardianFiller().Create();

        private static Contact CreateRandomContact() =>
            CreateRandomContactFiller().Create();

        private async ValueTask<Guardian> PostRandomGuardianAsync()
        {
            Guardian randomGuardian = CreateRandomGuardian();
            await this.otripleSApiBroker.PostGuardianAsync(randomGuardian);

            return randomGuardian;
        }

        private async ValueTask<Contact> PostRandomContactAsync()
        {
            Contact randomContact = CreateRandomContact();
            await this.otripleSApiBroker.PostContactAsync(randomContact);

            return randomContact;
        }

        private async ValueTask<GuardianContact> DeleteGuardianContactAsync(
            GuardianContact guardianContact)
        {
            GuardianContact deletedGuardianContact =
                await this.otripleSApiBroker.DeleteGuardianContactByIdAsync(
                    guardianContact.GuardianId,
                    guardianContact.ContactId);

            return deletedGuardianContact;
        }

        private static Filler<Contact> CreateRandomContactFiller()
        {
            Filler<Contact> filler = new Filler<Contact>();
            Guid randomCreatedUpdatedById = Guid.NewGuid();

            filler.Setup()
                .OnProperty(contact => contact.CreatedBy).Use(randomCreatedUpdatedById)
                .OnProperty(contact => contact.UpdatedBy).Use(randomCreatedUpdatedById)
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }

        private Filler<Guardian> CreateRandomGuardianFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Guardian>();

            filler.Setup()
                .OnProperty(guardian => guardian.CreatedBy).Use(posterId)
                .OnProperty(guardian => guardian.UpdatedBy).Use(posterId)
                .OnProperty(guardian => guardian.CreatedDate).Use(now)
                .OnProperty(guardian => guardian.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
