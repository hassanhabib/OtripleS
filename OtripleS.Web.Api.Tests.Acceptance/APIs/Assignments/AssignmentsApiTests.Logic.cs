// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Assignments;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Assignments
{
    public partial class AssignmentsApiTests
    {
        [Fact]
        public async Task ShouldGetAllAssignmentsAsync()
        {
            // given
            var randomAssignments = new List<Assignment>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                randomAssignments.Add(await PostRandomAssignmentAsync());
            }

            List<Assignment> inputAssignments = randomAssignments;
            List<Assignment> expectedAssignments = inputAssignments.ToList();

            // when
            List<Assignment> actualAssignments = 
                await this.otripleSApiBroker.GetAllAssignmentsAsync();

            // then
            foreach (Assignment expectedAssignment in expectedAssignments)
            {
                Assignment actualAssignment = 
                    actualAssignments.Single(assignment => 
                        assignment.Id == expectedAssignment.Id);

                actualAssignment.Should().BeEquivalentTo(expectedAssignment);
                await this.otripleSApiBroker.DeleteAssignmentByIdAsync(actualAssignment.Id);
            }
        }

        [Fact]
        public async Task ShouldModifyAssignmentAsync()
        {
            // given
            Assignment randomAssignment = await PostRandomAssignmentAsync();
            Assignment modifiedAssignment = UpdateAssignmentRandom(randomAssignment);

            // when
            await this.otripleSApiBroker.PutAssignmentAsync(modifiedAssignment);

            Assignment actualAssignment =
                await this.otripleSApiBroker.GetAssignmentByIdAsync(randomAssignment.Id);

            // then
            actualAssignment.Should().BeEquivalentTo(modifiedAssignment);
            await this.otripleSApiBroker.DeleteAssignmentByIdAsync(actualAssignment.Id);
        }

        [Fact]
        public async Task ShouldPostAssignmentAsync()
        {
            // given
            Assignment randomAssignment = CreateRandomAssignment();
            Assignment inputAssignment = randomAssignment;
            Assignment expectedAssignment = inputAssignment;

            // when 
            await this.otripleSApiBroker.PostAssignmentAsync(inputAssignment);

            Assignment actualAssignment =
                await this.otripleSApiBroker.GetAssignmentByIdAsync(inputAssignment.Id);

            // then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);
            await this.otripleSApiBroker.DeleteAssignmentByIdAsync(actualAssignment.Id);
        }

        [Fact]
        public async Task ShouldDeleteAssignmentAsync()
        {
            // given
            Assignment randomAssignment = await PostRandomAssignmentAsync();
            Assignment inputAssignment = randomAssignment;
            Assignment expectedAssignment = inputAssignment;

            // when 
            Assignment deletedAssignment =
                await this.otripleSApiBroker.DeleteAssignmentByIdAsync(inputAssignment.Id);

            ValueTask<Assignment> getAssignmentByIdTask =
                this.otripleSApiBroker.GetAssignmentByIdAsync(inputAssignment.Id);

            // then
            deletedAssignment.Should().BeEquivalentTo(expectedAssignment);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getAssignmentByIdTask.AsTask());
        }
    }
}