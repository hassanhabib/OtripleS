// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Services.Foundations.Teachers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Teachers
{
    public partial class TeacherServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITeacherService teacherService;

        public TeacherServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.teacherService = new TeacherService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IEnumerable<Teacher> CreateRandomTeachers(DateTimeOffset dateTime) =>
            CreateRandomTeacherFiller(dateTime).Create(GetRandomNumber());

        private static Teacher CreateRandomTeacher(DateTimeOffset dateTime) =>
            CreateRandomTeacherFiller(dateTime).Create();

        private static IQueryable<Teacher> CreateRandomTeachers() =>
            CreateRandomTeacherFiller(dates: DateTimeOffset.UtcNow).Create(GetRandomNumber()).AsQueryable();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

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

        private static Filler<Teacher> CreateRandomTeacherFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Teacher>();

            filler.Setup()
                .OnProperty(teacher => teacher.Status).Use(TeacherStatus.Active)
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(teacher => teacher.SemesterCourses).IgnoreIt()
                .OnProperty(teacher => teacher.TeacherContacts).IgnoreIt()
                .OnProperty(teacher => teacher.ReviewedStudentExams).IgnoreIt()
                .OnProperty(teacher => teacher.TeacherAttachments).IgnoreIt();

            return filler;
        }
    }
}