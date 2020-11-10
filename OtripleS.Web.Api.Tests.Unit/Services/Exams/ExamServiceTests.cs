// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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

		private static DateTimeOffset GetRandomDateTime() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private Exam CreateRandomExam(DateTimeOffset dateTime) =>
			CreateRandomExamFiller(dateTime).Create();

		private Filler<Exam> CreateRandomExamFiller(DateTimeOffset dateTime)
		{
			var filler = new Filler<Exam>();

			filler.Setup()
				.OnType<DateTimeOffset>().Use(dateTime);

			return filler;
		}
	}
}