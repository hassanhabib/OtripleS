// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldCreateAssignmentAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Assignment randomAssignment = CreateRandomAssignment(randomDateTime);
            randomAssignment.UpdatedBy = randomAssignment.CreatedBy;
            randomAssignment.UpdatedDate = randomAssignment.CreatedDate;
            Assignment inputAssignment = randomAssignment;
            Assignment storageAssignment = randomAssignment;
            Assignment expectedAssignment = storageAssignment;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(inputAssignment))
                    .ReturnsAsync(storageAssignment);

            // when
            Assignment actualAssignment =
                await this.assignmentService.CreateAssignmentAsync(inputAssignment);

            // then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(inputAssignment),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllAssignments()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Assignment> randomAssignments = CreateRandomAssignments(randomDateTime);
            IQueryable<Assignment> storageAssignments = randomAssignments;
            IQueryable<Assignment> expectedAssignments = storageAssignments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignments())
                    .Returns(storageAssignments);

            // when
            IQueryable<Assignment> actualAssignments =
                this.assignmentService.RetrieveAllAssignments();

            // then
            actualAssignments.Should().BeEquivalentTo(expectedAssignments);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveAssignmentById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Guid inputAssignmentId = randomAssignment.Id;
            Assignment inputAssignment = randomAssignment;
            Assignment expectedAssignment = randomAssignment;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAssignmentByIdAsync(inputAssignmentId))
                .ReturnsAsync(inputAssignment);

            //when 
            Assignment actualAssignment = await this.assignmentService.RetrieveAssignmentById(inputAssignmentId);

            //then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
