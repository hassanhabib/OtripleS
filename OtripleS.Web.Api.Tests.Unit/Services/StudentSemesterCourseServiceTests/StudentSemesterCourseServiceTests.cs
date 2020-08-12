using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Services.StudentSemesterCourses;
using System;
using System.Collections.Generic;
using System.Text;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourseServiceTests
{
    public partial class StudentSemesterCourseServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly StudentSemesterCourseService studentSemesterCourseService;

        public StudentSemesterCourseServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.studentSemesterCourseService = new StudentSemesterCourseService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private StudentSemesterCourse CreateRandomStudentSemesterCourse(DateTimeOffset dates) =>
            CreateStudentSemesterCourseFiller(dates).Create();

        private static Filler<StudentSemesterCourse> CreateStudentSemesterCourseFiller(DateTimeOffset dates)
        {
            var filler = new Filler<StudentSemesterCourse>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(dates)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(dates);

            return filler;
        }
    }
}
