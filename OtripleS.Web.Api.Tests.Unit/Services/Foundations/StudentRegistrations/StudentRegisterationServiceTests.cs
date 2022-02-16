// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
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
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Services.Foundations.StudentRegistrations;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IStudentRegistrationService studentRegistrationService;

        public StudentRegistrationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.studentRegistrationService = new StudentRegistrationService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private StudentRegistration CreateRandomStudentRegistration() =>
            CreateStudentRegistrationFiller().Create();

        private static IQueryable<StudentRegistration> CreateRandomStudentRegistrations(DateTimeOffset dates) =>
            CreateStudentRegistrationFiller().Create(GetRandomNumber()).AsQueryable();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static Filler<StudentRegistration> CreateStudentRegistrationFiller()
        {
            var filler = new Filler<StudentRegistration>();

            filler.Setup()
                .OnProperty(StudentRegistration => StudentRegistration.Student).IgnoreIt()
                .OnProperty(StudentRegistration => StudentRegistration.Registration).IgnoreIt();

            return filler;
        }
    }
}
