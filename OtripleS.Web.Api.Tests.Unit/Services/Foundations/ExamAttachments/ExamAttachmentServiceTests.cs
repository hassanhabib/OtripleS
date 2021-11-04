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
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Services.Foundations.ExamAttachments;
using Tynamix.ObjectFiller;
using Xeptions;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IExamAttachmentService examAttachmentService;

        public ExamAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.examAttachmentService = new ExamAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

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
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static ExamAttachment CreateRandomExamAttachment() =>
            CreateExamAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static ExamAttachment CreateRandomExamAttachment(DateTimeOffset dates) =>
            CreateExamAttachmentFiller(dates).Create();

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static IQueryable<ExamAttachment> CreateRandomExamAttachments() =>
            CreateExamAttachmentFiller(DateTimeOffset.UtcNow)
                .Create(GetRandomNumber()).AsQueryable();               

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private static Filler<ExamAttachment> CreateExamAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<ExamAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(ExamAttachment => ExamAttachment.Exam).IgnoreIt()
                .OnProperty(ExamAttachment => ExamAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
