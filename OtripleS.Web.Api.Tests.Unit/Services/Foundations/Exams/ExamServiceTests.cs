﻿// ---------------------------------------------------------------
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
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Services.Foundations.Exams;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Exams
{
    public partial class ExamServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IExamService examService;

        public ExamServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.examService = new ExamService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

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

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<Exam> CreateRandomExams(DateTimeOffset dateTime) =>
            CreateRandomExamFiller(dateTime: dateTime).Create(GetRandomNumber()).AsQueryable();

        private static Exam CreateRandomExam(DateTimeOffset dateTime) =>
            CreateRandomExamFiller(dateTime).Create();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                 actualException.Message == expectedException.Message
                 && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message &&
                expectedException.InnerException.Message == actualException.InnerException.Message &&
                (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static ExamType GetInValidExamType()
        {
            int maxExamType =
                (int)Enum.GetValues(typeof(ExamType))
                    .Cast<ExamType>().Count();

            int randomOutOfRangeEnumValue = new IntRange(
                min: maxExamType,
                max: maxExamType + GetRandomNumber())
                    .GetValue();

            return (ExamType)randomOutOfRangeEnumValue;
        }

        private static Filler<Exam> CreateRandomExamFiller(DateTimeOffset dateTime)
        {
            var filler = new Filler<Exam>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTime)
                .OnProperty(exam => exam.SemesterCourse).IgnoreIt()
                .OnProperty(exam => exam.StudentExams).IgnoreIt()
                .OnProperty(exam => exam.ExamAttachments).IgnoreIt()
                .OnProperty(exam => exam.ExamFees).IgnoreIt();

            return filler;
        }
    }
}