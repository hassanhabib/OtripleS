//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Services.StudentAttachments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IStudentAttachmentService studentAttachmentService;

        public StudentAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.studentAttachmentService = new StudentAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private StudentAttachment CreateRandomStudentAttachment() =>
            CreateStudentAttachmentFiller().Create();

        private static Filler<StudentAttachment> CreateStudentAttachmentFiller()
        {
            var filler = new Filler<StudentAttachment>();
            filler.Setup()
                .OnProperty(studentAttachment => studentAttachment.Student).IgnoreIt()
                .OnProperty(studentAttachment => studentAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
