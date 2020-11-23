// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.Contacts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ContactsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public ContactsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Contact CreateRandomContact() =>
            CreateRandomContactFiller().Create();

        private IEnumerable<Contact> CreateRandomContacts() =>
            CreateRandomContactFiller().Create(GetRandomNumber());

        private Contact UpdateContactRandom(Contact contact)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Contact>();

            filler.Setup()
                .OnProperty(contact => contact.Id).Use(contact.Id)
                .OnProperty(contact => contact.IsPrimary).Use(contact.IsPrimary)
                .OnProperty(contact => contact.Type).Use(contact.Type)
                .OnProperty(contact => contact.Information).Use(contact.Information)
                .OnProperty(contact => contact.Notes).Use(contact.Notes)
                .OnProperty(contact => contact.CreatedBy).Use(contact.CreatedBy)
                .OnProperty(contact => contact.UpdatedBy).Use(contact.UpdatedBy)
                .OnProperty(contact => contact.CreatedDate).Use(contact.CreatedDate)
                .OnProperty(contact => contact.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
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
    }
}