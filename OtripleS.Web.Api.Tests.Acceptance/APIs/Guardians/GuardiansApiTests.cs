using System;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Guardians
{
	[Collection(nameof(ApiTestCollection))]
	public partial class GuardiansApiTests
	{
		private readonly OtripleSApiBroker otripleSApiBroker;

		public GuardiansApiTests(OtripleSApiBroker otripleSApiBroker) =>
			this.otripleSApiBroker = otripleSApiBroker;

		private Guardian CreateRandomGuardian() =>
			CreateRandomGuardianFiller().Create();

		private Filler<Guardian> CreateRandomGuardianFiller()
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;
			Guid posterId = Guid.NewGuid();
			var filler = new Filler<Guardian>();

			filler.Setup()
				.OnProperty(guardian => guardian.CreatedBy).Use(posterId)
				.OnProperty(guardian => guardian.UpdatedBy).Use(posterId)
				.OnProperty(guardian => guardian.CreatedDate).Use(now)
				.OnProperty(guardian => guardian.UpdatedDate).Use(now)
				.OnType<DateTimeOffset>().Use(GetRandomDateTime());

			return filler;
		}

		private static DateTimeOffset GetRandomDateTime() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();
	}
}
