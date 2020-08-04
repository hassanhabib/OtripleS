using System;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Services.SemesterCourses;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourseServiceTests
{
    public partial class SemesterCourseServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private SemesterCourseService semesterCourseService;

        public SemesterCourseServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.semesterCourseService = new SemesterCourseService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }
        
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        
        private Filler<SemesterCourse> CreateRandomSemesterCourseFiller(DateTimeOffset dateTime)
        {
            var filler = new Filler<SemesterCourse>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime);

            return filler;
        }
        
        private SemesterCourse CreateRandomSemesterCourse(DateTimeOffset dateTime) =>
            CreateRandomSemesterCourseFiller(dateTime).Create();
    }
}