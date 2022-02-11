// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Services.Foundations.Attachments;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly AttachmentService attachmentService;

        public AttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.attachmentService = new AttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<Attachment> CreateRandomAttachments() =>
            CreateAttachmentFiller(dates: DateTimeOffset.UtcNow)
            .Create(GetRandomNumber()).AsQueryable();

        private static Attachment CreateRandomAttachment() =>
            CreateAttachmentFiller(dates: DateTimeOffset.UtcNow).Create();

        private static Attachment CreateRandomAttachment(DateTimeOffset dates) =>
            CreateAttachmentFiller(dates).Create();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

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

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static Filler<Attachment> CreateAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Attachment>();

            filler.Setup()
                .OnProperty(attachment => attachment.CreatedDate).Use(dates)
                .OnProperty(attachment => attachment.UpdatedDate).Use(dates)
                .OnProperty(attachment => attachment.StudentAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.GuardianAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.TeacherAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.CalendarEntryAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.CourseAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.ExamAttachments).IgnoreIt()
                .OnProperty(attachment => attachment.AssignmentAttachments).IgnoreIt();

            return filler;
        }
    }
}
