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
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Foundations.StudentExamFees;
using OtripleS.Web.Api.Services.Foundations.StudentExamFees;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IStudentExamFeeService studentExamFeeService;

        public StudentExamFeeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.studentExamFeeService = new StudentExamFeeService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }
        private static StudentExamFee CreateRandomStudentExamFee() =>
           CreateStudentExamFeeFiller(DateTimeOffset.UtcNow).Create();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static IQueryable<StudentExamFee> CreateRandomStudentExamFees() =>
            CreateStudentExamFeeFiller(DateTimeOffset.UtcNow)
                .Create(GetRandomNumber()).AsQueryable();

        private static Filler<StudentExamFee> CreateStudentExamFeeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<StudentExamFee>();

            filler.Setup()
                .OnProperty(studentExamFee => studentExamFee.CreatedDate).Use(dates)
                .OnProperty(studentExamFee => studentExamFee.UpdatedDate).Use(dates)
                .OnProperty(studentExamFee => studentExamFee.Student).IgnoreIt()
                .OnProperty(studentExamFee => studentExamFee.ExamFee).IgnoreIt()
                .OnProperty(studentExamFee => studentExamFee.CreatedByUser).IgnoreIt()
                .OnProperty(studentExamFee => studentExamFee.UpdatedByUser).IgnoreIt();

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static StudentExamFee CreateRandomStudentExamFee(DateTimeOffset dates) =>
            CreateStudentExamFeeFiller(dates).Create();

        private static SqlException GetSqlException() =>
        (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        public static TheoryData InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new TheoryData<int>
            {
                randomMoreThanMinuteFromNow,
                randomMoreThanMinuteBeforeNow
            };
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();
    }
}
