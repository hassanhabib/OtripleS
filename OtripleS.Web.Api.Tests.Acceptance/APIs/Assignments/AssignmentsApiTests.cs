// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Assignments;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Assignments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class AssignmentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public AssignmentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static Assignment CreateRandomAssignment() =>
            CreateRandomAssignmentFiller().Create();

        private async ValueTask<Assignment> PostRandomAssignmentAsync()
        {
            Assignment randomAssignment = CreateRandomAssignment();
            await this.otripleSApiBroker.PostAssignmentAsync(randomAssignment);

            return randomAssignment;
        }

        private static Assignment UpdateAssignmentRandom(Assignment inputAssignment)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            var filler = new Filler<Assignment>();

            filler.Setup()
                .OnProperty(assignment => assignment.Id).Use(inputAssignment.Id)
                .OnProperty(assignment => assignment.CreatedBy).Use(inputAssignment.CreatedBy)
                .OnProperty(assignment => assignment.UpdatedBy).Use(inputAssignment.UpdatedBy)
                .OnProperty(assignment => assignment.CreatedDate).Use(inputAssignment.CreatedDate)
                .OnProperty(assignment => assignment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Assignment> CreateRandomAssignmentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Assignment>();

            filler.Setup()
                .OnProperty(assignment => assignment.CreatedBy).Use(posterId)
                .OnProperty(assignment => assignment.UpdatedBy).Use(posterId)
                .OnProperty(assignment => assignment.CreatedDate).Use(now)
                .OnProperty(assignment => assignment.UpdatedDate).Use(now)
                .OnProperty(assignment => assignment.Deadline).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}