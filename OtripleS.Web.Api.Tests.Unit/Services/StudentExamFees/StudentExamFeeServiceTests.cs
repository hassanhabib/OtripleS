//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Services.StudentExamFees;
using OtripleS.Web.Api.Services.StudentStudentExamFees;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
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

        private StudentExamFee CreateRandomStudentExamFee() =>
            CreateStudentExamFeeFiller(DateTimeOffset.UtcNow).Create();

        private static Filler<StudentExamFee> CreateStudentExamFeeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<StudentExamFee>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(studentExamFee => studentExamFee.CreatedByUser).IgnoreIt()
                .OnProperty(studentExamFee => studentExamFee.UpdatedByUser).IgnoreIt()
                .OnProperty(studentExamFee => studentExamFee.Student).IgnoreIt()
                .OnProperty(studentExamFee => studentExamFee.ExamFee).IgnoreIt();

            return filler;
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private StudentExamFee CreateRandomStudentExamFee(DateTimeOffset dates) =>
            CreateStudentExamFeeFiller(dates).Create();

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

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();
    }
}
