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
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Services.StudentContacts;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IStudentContactService studentContactService;

        public StudentContactServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.studentContactService = new StudentContactService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private StudentContact CreateRandomStudentContact() =>
            CreateStudentContactFiller(DateTimeOffset.UtcNow).Create();

        private IQueryable<StudentContact> CreateRandomStudentContacts() =>
            CreateStudentContactFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

        private StudentContact CreateRandomStudentContact(DateTimeOffset dates) =>
            CreateStudentContactFiller(dates).Create();

        private static Filler<StudentContact> CreateStudentContactFiller(DateTimeOffset dates)
        {
            var filler = new Filler<StudentContact>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(studentcontact => studentcontact.Student).IgnoreIt()
                .OnProperty(studentcontact => studentcontact.Contact).IgnoreIt();

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