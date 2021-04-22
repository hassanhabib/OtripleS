// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Services.StudentExamFees;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IStudentExamFeeService StudentExamFeeService;

        public StudentExamFeeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.StudentExamFeeService = new StudentExamFeeService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }
        private StudentExamFee CreateRandomStudentExamFee() =>
           CreateStudentExamFeeFiller(DateTimeOffset.UtcNow).Create();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

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
    }
}
