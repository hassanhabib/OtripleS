//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Services.ExamAttachments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
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

        private ExamAttachment CreateRandomExamAttachment() =>
            CreateExamAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static Filler<ExamAttachment> CreateExamAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<ExamAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(examAttachment => examAttachment.Exam).IgnoreIt()
                .OnProperty(examAttachment => examAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
