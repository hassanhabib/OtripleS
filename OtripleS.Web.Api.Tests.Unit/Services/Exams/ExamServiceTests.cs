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
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Services.Exams;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
		private readonly IExamService examService;

		public ExamServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();
			this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

			this.examService = new ExamService(
				storageBroker: this.storageBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object,
				dateTimeBroker: this.dateTimeBrokerMock.Object);
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

		private static DateTimeOffset GetRandomDateTime() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private IQueryable<Exam> CreateRandomExams(DateTimeOffset dateTime) =>
			CreateRandomExamFiller(dateTime: dateTime).Create(GetRandomNumber()).AsQueryable();

		private Exam CreateRandomExam(DateTimeOffset dateTime) =>
			CreateRandomExamFiller(dateTime).Create();

		private static SqlException GetSqlException() =>
			(SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

		private Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
		{
			return actualException =>
				expectedException.Message == actualException.Message &&
				expectedException.InnerException.Message == actualException.InnerException.Message;
		}

		private ExamType GetInValidExamType()
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

		private Filler<Exam> CreateRandomExamFiller(DateTimeOffset dateTime)
		{
			var filler = new Filler<Exam>();

			filler.Setup()
				.OnType<DateTimeOffset>().Use(dateTime)
				.OnProperty(exam => exam.SemesterCourse).IgnoreIt()
				.OnProperty(exam => exam.StudentExams).IgnoreIt();

			return filler;
		}
	}
}