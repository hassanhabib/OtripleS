// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Services.Users;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserServiceTests
{
    public partial class UserServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.userService = new UserService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private User CreateRandomUser(DateTimeOffset dates) =>
            CreateUserFiller(dates).Create();

        private static Filler<User> CreateUserFiller(DateTimeOffset dates)
        {
            var filler = new Filler<User>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }

        private static string GetRandomPassword() => new MnemonicString(1,8,20).GetValue();
    }
}
