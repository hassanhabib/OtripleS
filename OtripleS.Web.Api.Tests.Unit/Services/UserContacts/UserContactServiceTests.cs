// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Services.UserContacts;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserContacts
{
    public partial class UserContactServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserContactService userContactService;

        public UserContactServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.userContactService = new UserContactService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private UserContact CreateRandomUserContact() =>
            CreateUserContactFiller(DateTimeOffset.UtcNow).Create();

        private IQueryable<UserContact> CreateRandomUserContacts() =>
            CreateUserContactFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

        private UserContact CreateRandomUserContact(DateTimeOffset dates) =>
            CreateUserContactFiller(dates).Create();

        private static Filler<UserContact> CreateUserContactFiller(DateTimeOffset dates)
        {
            var filler = new Filler<UserContact>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(usercontact => usercontact.User).IgnoreIt()
                .OnProperty(usercontact => usercontact.Contact).IgnoreIt();

            return filler;
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }
    }
}
