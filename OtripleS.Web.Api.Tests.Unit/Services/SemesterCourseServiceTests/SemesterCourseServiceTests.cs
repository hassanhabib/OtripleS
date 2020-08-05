// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
        private readonly ISemesterCourseService semesterCourseService;

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

        private SemesterCourse CreateRandomSemesterCourse(DateTimeOffset dates) =>
            CreateSemesterCourseFiller(dates).Create();

        private static Filler<SemesterCourse> CreateSemesterCourseFiller(DateTimeOffset dates)
        {
            var filler = new Filler<SemesterCourse>();
            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.StartDate).Use(GetRandomDateTime())
                .OnProperty(semesterCourse => semesterCourse.EndDate).Use(GetRandomDateTime())
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(dates)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(dates);

            return filler;
        }
    }
}
