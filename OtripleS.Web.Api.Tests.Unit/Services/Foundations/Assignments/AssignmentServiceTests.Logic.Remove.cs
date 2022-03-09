// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAssignmentAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dates: dateTime);
            Guid inputAssignmentId = randomAssignment.Id;
            Assignment inputAssignment = randomAssignment;
            Assignment storageAssignment = inputAssignment;
            Assignment expectedAssignment = storageAssignment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId))
                    .ReturnsAsync(inputAssignment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAssignmentAsync(inputAssignment))
                    .ReturnsAsync(storageAssignment);

            // when
            Assignment actualAssignment =
                await this.assignmentService.RemoveAssignmentByIdAsync(inputAssignmentId);

            // then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAsync(inputAssignment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
