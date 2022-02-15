// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Services.Foundations.GuardianContacts;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IGuardianContactService guardianContactService;

        public GuardianContactServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.guardianContactService = new GuardianContactService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static GuardianContact CreateRandomGuardianContact() =>
            CreateGuardianContactFiller(DateTimeOffset.UtcNow).Create();

        private static IQueryable<GuardianContact> CreateRandomGuardianContacts() =>
            CreateGuardianContactFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

        private static GuardianContact CreateRandomGuardianContact(DateTimeOffset dates) =>
            CreateGuardianContactFiller(dates).Create();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomMessage() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Filler<GuardianContact> CreateGuardianContactFiller(DateTimeOffset dates)
        {
            var filler = new Filler<GuardianContact>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(guardiancontact => guardiancontact.Guardian).IgnoreIt()
                .OnProperty(guardiancontact => guardiancontact.Contact).IgnoreIt();

            return filler;
        }
    }
}