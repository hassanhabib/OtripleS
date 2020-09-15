// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Services.StudentGuardians;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardianServiceTests
{
	public partial class StudentGuardianServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
		private readonly IStudentGuardianService studentGuardianService;

		public StudentGuardianServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();
			this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

			this.studentGuardianService = new StudentGuardianService(
				storageBroker: this.storageBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object,
				dateTimeBroker: this.dateTimeBrokerMock.Object);
		}
		private static DateTimeOffset GetRandomDateTime() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private StudentGuardian CreateRandomStudentGuardian(DateTimeOffset dates) =>
			CreateStudentGuardianFiller(dates).Create();

		private static Filler<StudentGuardian> CreateStudentGuardianFiller(DateTimeOffset dates)
		{
			var filler = new Filler<StudentGuardian>();
			filler.Setup()
				.OnType<DateTimeOffset>().Use(dates)
				.OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(dates)
				.OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(dates);

			return filler;
		}

		private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
		{
			return actualException =>
				expectedException.Message == actualException.Message
				&& expectedException.InnerException.Message == actualException.InnerException.Message;
		}

		private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
	}
}
