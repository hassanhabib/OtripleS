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
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Services.Foundations.StudentSemesterCourses;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IStudentSemesterCourseService studentSemesterCourseService;

        public StudentSemesterCourseServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.studentSemesterCourseService = new StudentSemesterCourseService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static StudentSemesterCourse CreateRandomStudentSemesterCourse(DateTimeOffset dates) =>
            CreateStudentSemesterCourseFiller(dates).Create();

        private static IQueryable<StudentSemesterCourse> CreateRandomStudentSemesterCourses() =>
            CreateStudentSemesterCourseFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

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

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetNegativeRandomNumber() =>
            -1 * GetRandomNumber();

        private static string GetRandomMessage() =>
            new MnemonicString().GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Filler<StudentSemesterCourse> CreateStudentSemesterCourseFiller(DateTimeOffset dates)
        {
            var filler = new Filler<StudentSemesterCourse>();
            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(dates)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(dates)
                .OnProperty(semesterCourse => semesterCourse.Student).IgnoreIt()
                .OnProperty(semesterCourse => semesterCourse.SemesterCourse).IgnoreIt();

            return filler;
        }
    }
}
