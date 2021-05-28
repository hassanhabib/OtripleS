// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Services.StudentRegistrations;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IStudentRegistrationService studentRegistrationService;

        public StudentRegistrationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.studentRegistrationService = new StudentRegistrationService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private StudentRegistration CreateRandomStudentRegistration() =>
            CreateStudentRegistrationFiller(DateTimeOffset.UtcNow).Create();

        private StudentRegistration CreateRandomStudentRegistration(DateTimeOffset dates) =>
            CreateStudentRegistrationFiller(dates).Create();

        private static Filler<StudentRegistration> CreateStudentRegistrationFiller(DateTimeOffset dates)
        {
            var filler = new Filler<StudentRegistration>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(StudentRegistration => StudentRegistration.Student).IgnoreIt()
                .OnProperty(StudentRegistration => StudentRegistration.Registration).IgnoreIt();
            return filler;
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static string GetRandomMessage() => new MnemonicString().GetValue();
    }
}
