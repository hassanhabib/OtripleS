// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenAssignmentsWasEmptyAndLogIt()
        {
            // given
            IQueryable<Assignment> emptyStorageAssignments = new List<Assignment>().AsQueryable();
            IQueryable<Assignment> expectedAssignments = emptyStorageAssignments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignments())
                    .Returns(expectedAssignments);

            // when
            IQueryable<Assignment> actualAssignments =
                this.assignmentService.RetrieveAllAssignments();

            // then
            actualAssignments.Should().BeEquivalentTo(emptyStorageAssignments);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No Assignments found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignments(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}