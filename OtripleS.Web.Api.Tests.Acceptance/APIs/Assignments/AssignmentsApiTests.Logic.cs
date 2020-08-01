using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Assignments
{
    public partial class AssignmentsApiTests
    {
        [Fact]
        public async Task ShouldGetAllAssignmentsAsync()
        {
            // given
            IEnumerable<Assignment> randomAssignments = GetRandomAssignments();
            IEnumerable<Assignment> inputAssignments = randomAssignments;

            foreach (Assignment assignment in inputAssignments)
            {
                await this.otripleSApiBroker.PostAssignmentAsync(assignment);
            }

            List<Assignment> expectedAssignments = inputAssignments.ToList();

            // when
            List<Assignment> actualAssignments = await this.otripleSApiBroker.GetAllAssignmentsAsync();

            // then
            foreach (Assignment expectedAssignment in expectedAssignments)
            {
                Assignment actualAssignment = actualAssignments.Single(assignment => assignment.Id == expectedAssignment.Id);
                actualAssignment.Should().BeEquivalentTo(expectedAssignment);
                await this.otripleSApiBroker.DeleteAssignmentByIdAsync(actualAssignment.Id);
            }
        }
        
        
    }
}