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
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Services.GuardianAttachmets;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IGuardianAttachmentService guardianAttachmentService;

        public GuardianAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.guardianAttachmentService = new GuardianAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: new DateTime()).GetValue();
         
        private GuardianAttachment CreateRandomGuardianAttachment() =>
            CreateGuardianAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private GuardianAttachment CreateRandomGuardianAttachment(DateTimeOffset dates) =>
           CreateGuardianAttachmentFiller(dates).Create();

        private IQueryable<GuardianAttachment> CreateRandomGuardianAttachments() =>
            CreateGuardianAttachmentFiller(dates: DateTimeOffset.UtcNow)
                .Create(GetRandomNumber()).AsQueryable();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();


        private static Filler<GuardianAttachment> CreateGuardianAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<GuardianAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(guardianAttachment => guardianAttachment.Guardian).IgnoreIt()
                .OnProperty(guardianAttachment => guardianAttachment.Attachment).IgnoreIt();

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
    }
}
