// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Services.Registrations;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using Xunit;

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

        private static Registration CreateRandomRegistration(DateTimeOffset dateTime) =>
            CreateRandomRegistrationFiller(dateTime).Create();

        public static TheoryData InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new TheoryData<int>
            {
                randomMoreThanMinuteFromNow ,
                randomMoreThanMinuteBeforeNow
            };
        }

        public static TheoryData InvalidEmailAddressCases()
        {
            string noString = null;
            string emptyString = string.Empty;
            string whiteSpaceString = "     ";
            string letterString = "NotAnEmail";
            string characterString = "\n\r\bnotanema1l76^8&";
            string domainString = "location.com";
            string incompleteEmailString = "hassan@piorsoft";

            return new TheoryData<string>
            {
                noString,
                emptyString,
                whiteSpaceString,
                letterString,
                characterString,
                domainString,
                incompleteEmailString
            };
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message &&
                expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static IQueryable<Registration> CreateRandomRegistrations(DateTimeOffset dates) =>
            CreateRandomRegistrationFiller(dates).Create(GetRandomNumber()).AsQueryable();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();
        private static string CreateRandomEmailAddress() => new EmailAddresses().GetValue();

        private static Filler<Registration> CreateRandomRegistrationFiller(DateTimeOffset dateTime)
        {
            var filler = new Filler<Registration>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime)
                .OnProperty(registration => registration.StudentEmail).Use(CreateRandomEmailAddress())
                .OnProperty(registration => registration.SubmitterEmail).Use(CreateRandomEmailAddress())
                .OnProperty(registration => registration.StudentPhone).Use(new PatternGenerator("{N:3}-{N:3}-{N:4}"))
                .OnProperty(registration => registration.SubmitterPhone).Use(new PatternGenerator("{N:3}-{N:3}-{N:4}"))
                .OnProperty(Registration => Registration.CreatedByUser).IgnoreIt()
                .OnProperty(Registration => Registration.UpdatedByUser).IgnoreIt()
                .OnProperty(Registration => Registration.StudentRegistrations).IgnoreIt();

            return filler;
        }
    }
}
