//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Services.StudentSemesterCourses;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IStudentSemesterCourseService studentSemesterCourseService;

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

        private IQueryable<StudentSemesterCourse> CreateRandomStudentSemesterCourses() =>
            CreateStudentSemesterCourseFiller(DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

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

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        public static IEnumerable<object[]> InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new List<object[]>
            {
                new object[] { randomMoreThanMinuteFromNow },
                new object[] { randomMoreThanMinuteBeforeNow }
            };
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    }
}
