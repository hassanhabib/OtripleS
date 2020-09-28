using System;
using System.Collections.Generic;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Acceptance.Contacts
{
    public partial class ContactsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public ContactsApiTests(OtripleSApiBroker otripleSApiBroker)
        {
            this.otripleSApiBroker = otripleSApiBroker;
        }
        
        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        
        private static Contact CreateRandomContact() =>
            CreateRandomContactFiller(GetRandomDateTime()).Create();
        private IEnumerable<Contact> CreateRandomContacts() =>
            CreateRandomContactFiller(GetRandomDateTime()).Create(GetRandomNumber());
        private static Filler<Contact> CreateRandomContactFiller(DateTimeOffset dateTime)
        {
            Filler<Contact> filler = new Filler<Contact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime);

            return filler;
        }
    }
}