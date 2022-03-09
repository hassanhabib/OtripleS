// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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
            this.loggingBrokerMock.VerifyNoOtherCalls();
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

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
            Assignment actualAssignment =
                await this.assignmentService.RetrieveAssignmentByIdAsync(inputAssignmentId);

            //then
            actualAssignment.Should().BeEquivalentTo(expectedAssignment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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
