//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Services.AssignmentAttachments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IAssignmentAttachmentService assignmentAttachmentService;

        public AssignmentAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.assignmentAttachmentService = new AssignmentAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private AssignmentAttachment CreateRandomAssignmentAttachment() =>
            CreateAssignmentAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private static Filler<AssignmentAttachment> CreateAssignmentAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<AssignmentAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(AssignmentAttachment => AssignmentAttachment.Assignment).IgnoreIt()
                .OnProperty(AssignmentAttachment => AssignmentAttachment.Attachment).IgnoreIt();

            return filler;
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }
    }
}
