//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Services.TeacherContacts;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITeacherContactService teacherContactService;

        public TeacherContactServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.teacherContactService = new TeacherContactService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private TeacherContact CreateRandomTeacherContact() =>
            CreateTeacherContactFiller(DateTimeOffset.UtcNow).Create();

        private IQueryable<TeacherContact> CreateRandomTeacherContacts() =>
            CreateTeacherContactFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

        private TeacherContact CreateRandomTeacherContact(DateTimeOffset dates) =>
            CreateTeacherContactFiller(dates).Create();

        private static Filler<TeacherContact> CreateTeacherContactFiller(DateTimeOffset dates)
        {
            var filler = new Filler<TeacherContact>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(teacher => teacher.Teacher).IgnoreIt()
                .OnProperty(teacher => teacher.Contact).IgnoreIt();

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