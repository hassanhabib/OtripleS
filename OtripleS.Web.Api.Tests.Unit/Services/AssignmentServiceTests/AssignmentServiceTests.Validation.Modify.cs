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
using EFxceptions.Models.Exceptions;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentServiceTests
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAssignmentIsNullAndLogItAsync()
        {
            //given
            Assignment invalidAssignment = null;
            var nullAssignmentException = new NullAssignmentException();

            var expectedAssignmentValidationException =
                new AssignmentValidationException(nullAssignmentException);

            //when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            //then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenAssignmentIdIsInvalidAndLogItAsync()
		{
			//given
			Guid invalidAssignmentId = Guid.Empty;
			DateTimeOffset dateTime = GetRandomDateTime();
			Assignment randomAssignment = CreateRandomAssignment(dateTime);
			Assignment invalidAssignment = randomAssignment;
			invalidAssignment.Id = invalidAssignmentId;

			var invalidAssignmentException = new InvalidAssignmentException(
				parameterName: nameof(Assignment.Id),
				parameterValue: invalidAssignment.Id);

			var expectedAssignmentValidationException =
				new AssignmentValidationException(invalidAssignmentException);

			//when
			ValueTask<Assignment> modifyAssignmentTask =
				this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

			//then
			await Assert.ThrowsAsync<AssignmentValidationException>(() =>
				modifyAssignmentTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
