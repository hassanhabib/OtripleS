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
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldModifyAssignmentAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(randomInputDate);
            Assignment inputAssignment = randomAssignment;
            Assignment afterUpdateStorageAssignment = inputAssignment;
            Assignment expectedAssignment = afterUpdateStorageAssignment;
            Assignment beforeUpdateStorageAssignment = randomAssignment.DeepClone();
            inputAssignment.UpdatedDate = randomDate;
            Guid assignmentId = inputAssignment.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId))
                    .ReturnsAsync(beforeUpdateStorageAssignment);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAssignmentAsync(inputAssignment))
                    .ReturnsAsync(afterUpdateStorageAssignment);

            // when
            Assignment actualAssignment =
                await this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAssignmentAsync(inputAssignment),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
