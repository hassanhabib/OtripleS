// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentServiceTests
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(sqlException);

            var badGuid = Guid.NewGuid();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(badGuid))
                    .Throws(sqlException);

            // when . then
            Assert.Throws<AssignmentDependencyException>(() =>
                this.assignmentService.RetrieveAssignmentById(badGuid));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(badGuid),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var guid = Guid.NewGuid();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(guid))
                    .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<AssignmentDependencyException>(() =>
                this.assignmentService.RetrieveAssignmentById(guid));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(guid),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedAssignmentServiceException =
                new AssignmentServiceException(exception);

            var guid = Guid.NewGuid();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(guid))
                    .Throws(exception);

            // when . then
            Assert.Throws<AssignmentServiceException>(() =>
                this.assignmentService.RetrieveAssignmentById(guid));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(guid),
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