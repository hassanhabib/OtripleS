// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Assignment> deleteAssignmentTask =
                this.assignmentService.RemoveAssignmentByIdAsync(someAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentDependencyException>(() =>
                deleteAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Assignment> deleteAssignmentTask =
                this.assignmentService.RemoveAssignmentByIdAsync(someAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentDependencyException>(() =>
                deleteAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedAssignmentException = new LockedAssignmentException(databaseUpdateConcurrencyException);

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(lockedAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Assignment> deleteAssignmentTask =
                this.assignmentService.RemoveAssignmentByIdAsync(someAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentDependencyException>(() =>
                deleteAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAssignmentId = Guid.NewGuid();
            var exception = new Exception();

            var expectedAssignmentServiceException =
                new AssignmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Assignment> deleteAssignmentTask =
                this.assignmentService.RemoveAssignmentByIdAsync(someAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentServiceException>(() =>
                deleteAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
