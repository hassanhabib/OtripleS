using System;
using System.Collections.Generic;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Assignments
{
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
        
        
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}