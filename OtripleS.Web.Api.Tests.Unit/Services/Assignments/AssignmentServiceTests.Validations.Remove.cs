// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAssignmentId = default;
            Guid inputAssignmentId = randomAssignmentId;

            var invalidAssignmentException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.Id),
                parameterValue: inputAssignmentId);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> actualAssignmentTask =
                this.assignmentService.RemoveAssignmentByIdAsync(inputAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() => actualAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAsync(It.IsAny<Assignment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStorageAssignmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dates: dateTimeOffset);
            Guid inputAssignmentId = randomAssignment.Id;
            Assignment inputAssignment = randomAssignment;
            Assignment nullStorageAssignment = null;

            var notFoundAssignmentException = new NotFoundAssignmentException(inputAssignmentId);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(notFoundAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId))
                    .ReturnsAsync(nullStorageAssignment);

            // when
            ValueTask<Assignment> actualAssignmentTask =
                this.assignmentService.RemoveAssignmentByIdAsync(inputAssignmentId);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() => actualAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(inputAssignmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAsync(It.IsAny<Assignment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
