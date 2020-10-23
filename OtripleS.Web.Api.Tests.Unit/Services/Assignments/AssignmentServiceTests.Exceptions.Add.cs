// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.UpdatedBy = inputAssignment.CreatedBy;
            inputAssignment.UpdatedDate = inputAssignment.CreatedDate;
            var sqlException = GetSqlException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(inputAssignment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentDependencyException>(() =>
                createAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(inputAssignment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.UpdatedBy = inputAssignment.CreatedBy;
            inputAssignment.UpdatedDate = inputAssignment.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(inputAssignment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentDependencyException>(() =>
                createAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(inputAssignment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.UpdatedBy = inputAssignment.CreatedBy;
            inputAssignment.UpdatedDate = inputAssignment.CreatedDate;
            var exception = new Exception();

            var expectedAssignmentServiceException =
                new AssignmentServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(inputAssignment))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Assignment> createAssignmentTask =
                 this.assignmentService.CreateAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentServiceException>(() =>
                createAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(inputAssignment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
