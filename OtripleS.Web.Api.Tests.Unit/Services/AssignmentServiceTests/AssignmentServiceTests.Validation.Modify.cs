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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAssignmentLabelIsInvalidAndLogItAsync(
                    string invalidAssignmentLabel)
        {
            // given
            Assignment randomAssignment = CreateRandomAssignment(DateTime.Now);
            Assignment invalidAssignment = randomAssignment;
            invalidAssignment.Label = invalidAssignmentLabel;

            var invalidAssignmentException = new InvalidAssignmentException(
               parameterName: nameof(Assignment.Label),
               parameterValue: invalidAssignment.Label);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAssignmentContentIsInvalidAndLogItAsync(
                    string invalidAssignmentContent)
        {
            // given
            Assignment randomAssignment = CreateRandomAssignment(DateTime.Now);
            Assignment invalidAssignment = randomAssignment;
            invalidAssignment.Content = invalidAssignmentContent;

            var invalidAssignmentException = new InvalidAssignmentException(
               parameterName: nameof(Assignment.Content),
               parameterValue: invalidAssignment.Content);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            // then
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
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.CreatedBy = default;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.CreatedBy),
                parameterValue: inputAssignment.CreatedBy);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.UpdatedBy = default;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.UpdatedBy),
                parameterValue: inputAssignment.UpdatedBy);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.CreatedDate = default;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.CreatedDate),
                parameterValue: inputAssignment.CreatedDate);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.UpdatedDate = default;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.UpdatedDate),
                parameterValue: inputAssignment.UpdatedDate);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenDeadlineIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.Deadline = default;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.Deadline),
                parameterValue: inputAssignment.Deadline);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
