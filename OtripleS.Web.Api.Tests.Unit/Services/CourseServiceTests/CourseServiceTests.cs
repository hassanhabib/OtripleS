using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Course;
using OtripleS.Web.Api.Services.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseServiceTests
{
    public partial class CourseServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly ICourseService courseService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;

        public CourseServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.courseService = new CourseService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }
        public Course CreateRandomCourse() =>
            CreateCourseFiller(dates: DateTimeOffset.UtcNow).Create();

        private Course CreateRandomCourse(DateTimeOffset dates) =>
            CreateCourseFiller(dates).Create();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<Course> CreateRandomCourses(DateTimeOffset dates) =>
            CreateCourseFiller(dates).Create(GetRandomNumber()).AsQueryable();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Course> CreateCourseFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Course>();
            filler.Setup()
                .OnProperty(course => course.CreatedDate).Use(dates)
                .OnProperty(course => course.UpdatedDate).Use(dates);

            return filler;
        }
    }
}
