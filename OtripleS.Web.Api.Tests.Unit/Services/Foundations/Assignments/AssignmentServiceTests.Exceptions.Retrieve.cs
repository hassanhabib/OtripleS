// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Foundations.Assignments;
using OtripleS.Web.Api.Models.Foundations.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogIt()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .Throws(sqlException);

            // when 
            ValueTask<Assignment> retrieveTask =
                this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentDependencyException>(() => retrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogIt()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Assignment> retrieveTask = this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentDependencyException>(() => retrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogIt()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();
            var exception = new Exception();

            var expectedAssignmentServiceException =
                new AssignmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .Throws(exception);

            // when 
            ValueTask<Assignment> retrieveTask =
                this.assignmentService.RetrieveAssignmentByIdAsync(someAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentServiceException>(() => retrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}