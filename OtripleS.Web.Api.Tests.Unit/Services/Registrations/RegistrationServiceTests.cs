// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Services.Registrations;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
{
    public partial class RegistrationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IRegistrationService registrationService;

        public RegistrationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.registrationService = new RegistrationService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Registration CreateRandomRegistration(DateTimeOffset dateTime) =>
            CreateRandomRegistrationFiller(dateTime).Create();

        private Filler<Registration> CreateRandomRegistrationFiller(DateTimeOffset dateTime)
        {
            var filler = new Filler<Registration>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime)
                .OnProperty(Registration => Registration.CreatedByUser).IgnoreIt()
                .OnProperty(Registration => Registration.UpdatedByUser).IgnoreIt();

            return filler;
        }
    }
}
