using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Services.CourseAttachments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICourseAttachmentService courseAttachmentService;

        public CourseAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.courseAttachmentService = new CourseAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private CourseAttachment CreateRandomCourseAttachment() =>
            CreateCourseAttachmentFiller(DateTimeOffset.UtcNow).Create();

        private CourseAttachment CreateRandomCourseAttachment(DateTimeOffset dates) =>
            CreateCourseAttachmentFiller(dates).Create();

        private static Filler<CourseAttachment> CreateCourseAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<CourseAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(courseAttachment => courseAttachment.Course).IgnoreIt()
                .OnProperty(courseAttachment => courseAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
