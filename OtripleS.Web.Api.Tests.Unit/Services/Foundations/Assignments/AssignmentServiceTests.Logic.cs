// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
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
    }
}
