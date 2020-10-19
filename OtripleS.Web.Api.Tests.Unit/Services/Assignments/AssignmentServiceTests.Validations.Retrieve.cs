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
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomAssignmentId = default;
            Guid inputAssignmentId = randomAssignmentId;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.Id),
                parameterValue: inputAssignmentId);

            var expectedAssignmentValidationException = new AssignmentValidationException(invalidAssignmentInputException);

            //when
            ValueTask<Assignment> retrieveAssignmentByIdTask =
                this.assignmentService.RetrieveAssignmentById(inputAssignmentId);

            //then
            await Assert.ThrowsAsync<AssignmentValidationException>(() => retrieveAssignmentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                Times.Once
            );

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageAssignmentIsNullAndLogItAsync()
        {
            //given
            Guid randomAssignmentId = Guid.NewGuid();
            Guid inputAssignmentId = randomAssignmentId;
            Assignment invalidStorageAssignment = null;

            var notFoundAssignmentException = new NotFoundAssignmentException(inputAssignmentId);

            var expectedAssignmentValidationException = new AssignmentValidationException(notFoundAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAssignmentByIdAsync(inputAssignmentId))
                .ReturnsAsync(invalidStorageAssignment);

            //when
            ValueTask<Assignment> retrieveAssignmentByIdTask =
                this.assignmentService.RetrieveAssignmentById(inputAssignmentId);

            //then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                retrieveAssignmentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}