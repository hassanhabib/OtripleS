using System;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Teachers
{
	[Collection(nameof(ApiTestCollection))]
	public partial class TeachersApiTests
	{
		private readonly OtripleSApiBroker otripleSApiBroker;

		public TeachersApiTests(OtripleSApiBroker otripleSApiBroker)
		{
			this.otripleSApiBroker = otripleSApiBroker;
		}

		private Teacher CreateRandomTeacher() =>
			CreateRandomTeacherFiller().Create();

		private Filler<Teacher> CreateRandomTeacherFiller()
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;
			Guid posterId = Guid.NewGuid();

			var filler = new Filler<Teacher>();

			filler.Setup()
				.OnProperty(teacher => teacher.CreatedBy).Use(posterId)
				.OnProperty(teacher => teacher.UpdatedBy).Use(posterId)
				.OnProperty(teacher => teacher.CreatedDate).Use(now)
				.OnProperty(teacher => teacher.UpdatedDate).Use(now)
				.OnType<DateTimeOffset>().Use(GetRandomDateTime());

			return filler;
		}
		private static DateTimeOffset GetRandomDateTime() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();
	}
}
