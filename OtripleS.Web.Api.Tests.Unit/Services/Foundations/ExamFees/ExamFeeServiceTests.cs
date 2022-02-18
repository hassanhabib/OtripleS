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
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Services.Foundations.ExamFees;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IExamFeeService examFeeService;

        public ExamFeeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.examFeeService = new ExamFeeService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static ExamFee CreateRandomExamFee() =>
            CreateExamFeeFiller(dates: GetRandomDateTime()).Create();

        private static IQueryable<ExamFee> CreateRandomExamFees() =>
            CreateExamFeeFiller(dates: GetRandomDateTime())
                .Create(GetRandomNumber()).AsQueryable();

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ExamFee CreateRandomExamFee(DateTimeOffset dates) =>
            CreateExamFeeFiller(dates).Create();

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

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();

        private static SqlException GetSqlException() =>
         (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Filler<ExamFee> CreateExamFeeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<ExamFee>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(examFee => examFee.Exam).IgnoreIt()
                .OnProperty(examFee => examFee.Fee).IgnoreIt()
                .OnProperty(examFee => examFee.CreatedByUser).IgnoreIt()
                .OnProperty(examFee => examFee.UpdatedByUser).IgnoreIt()
                .OnProperty(examFee => examFee.StudentExamFees).IgnoreIt();

            return filler;
        }
    }
}