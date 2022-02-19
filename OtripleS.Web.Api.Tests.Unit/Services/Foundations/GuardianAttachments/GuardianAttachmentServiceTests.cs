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
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Services.Foundations.GuardianAttachments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IGuardianAttachmentService guardianAttachmentService;

        public GuardianAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.guardianAttachmentService = new GuardianAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static GuardianAttachment CreateRandomGuardianAttachment() =>
            CreateGuardianAttachmentFiller(dates: GetRandomDateTime()).Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<GuardianAttachment> CreateRandomGuardianAttachments() =>
            CreateGuardianAttachmentFiller(dates:  GetRandomDateTime())
                .Create(GetRandomNumber()).AsQueryable();

        private static GuardianAttachment CreateRandomGuardianAttachment(DateTimeOffset dates) =>
            CreateGuardianAttachmentFiller(dates).Create();

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();


        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Filler<GuardianAttachment> CreateGuardianAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<GuardianAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(guardianAttachment => guardianAttachment.Guardian).IgnoreIt()
                .OnProperty(guardianAttachment => guardianAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
