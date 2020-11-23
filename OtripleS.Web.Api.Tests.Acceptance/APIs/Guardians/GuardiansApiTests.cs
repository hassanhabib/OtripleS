// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Guardians;
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

        private IEnumerable<Guardian> GetRandomGuardians() =>
            CreateRandomGuardianFiller().Create(GetRandomNumber());

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

        private Guardian UpdateGuardianRandom(Guardian guardian)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Guardian>();

            filler.Setup()
                .OnProperty(guardian => guardian.Id).Use(guardian.Id)
                .OnProperty(guardian => guardian.CreatedBy).Use(guardian.CreatedBy)
                .OnProperty(guardian => guardian.UpdatedBy).Use(guardian.UpdatedBy)
                .OnProperty(guardian => guardian.CreatedDate).Use(guardian.CreatedDate)
                .OnProperty(guardian => guardian.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 10).GetValue();
    }
}
