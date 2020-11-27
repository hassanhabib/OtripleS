// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Assignments;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Assignments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class AssignmentsApiTests
    {
        private OtripleSApiBroker otripleSApiBroker;

        public AssignmentsApiTests(OtripleSApiBroker otripleSApiBroker)
        {
            this.otripleSApiBroker = otripleSApiBroker;
        }
        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private IEnumerable<Assignment> GetRandomAssignments() =>
            CreateRandomAssignmentFiller().Create(GetRandomNumber());

        private Assignment CreateRandomAssignment() =>
            CreateRandomAssignmentFiller().Create();

        private Filler<Assignment> CreateRandomAssignmentFiller()
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

        private Assignment UpdateAssignmentRandom(Assignment inputAssignment)
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
    }
}