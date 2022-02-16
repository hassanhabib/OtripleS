// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedAssignmentDependencyException =
                new AssignmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignments())
                    .Throws(sqlException);

            // when
            Action retrieveAllAssignmentsAction = () =>
                this.assignmentService.RetrieveAllAssignments();

            // then
            Assert.Throws<AssignmentDependencyException>(
                retrieveAllAssignmentsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAssignmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedAssignmentServiceException =
                new AssignmentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignments())
                    .Throws(serviceException);

            // when
            Action retrieveAllAssignmentsAction = () =>
                this.assignmentService.RetrieveAllAssignments();

            // then
            Assert.Throws<AssignmentServiceException>(
               retrieveAllAssignmentsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}