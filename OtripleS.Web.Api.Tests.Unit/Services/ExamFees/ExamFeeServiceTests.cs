//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Services.ExamFees;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IExamFeeService examFeeService;

        public ExamFeeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.examFeeService = new ExamFeeService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private ExamFee CreateRandomExamFee() =>
            CreateExamFeeFiller(DateTimeOffset.UtcNow).Create();

        private IQueryable<ExamFee> CreateRandomExamFees() =>
            CreateExamFeeFiller(DateTimeOffset.UtcNow)
                .Create(GetRandomNumber()).AsQueryable();

        private static Filler<ExamFee> CreateExamFeeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<ExamFee>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(examFee => examFee.Exam).IgnoreIt()
                .OnProperty(examFee => examFee.Fee).IgnoreIt()
                .OnProperty(examFee => examFee.CreatedByUser).IgnoreIt()
                .OnProperty(examFee => examFee.UpdatedByUser).IgnoreIt();

            return filler;
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private ExamFee CreateRandomExamFee(DateTimeOffset dates) =>
            CreateExamFeeFiller(dates).Create();

        public static IEnumerable<object[]> InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new List<object[]>
            {
                new object[] { randomMoreThanMinuteFromNow },
                new object[] { randomMoreThanMinuteBeforeNow }
            };
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();

        private static SqlException GetSqlException() =>
         (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    }
}
