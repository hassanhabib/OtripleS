// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAssignmentIsNullAndLogItAsync()
        {
            // given
            Assignment randomAssignment = null;
            Assignment nullAssignment = randomAssignment;
            var nullAssignmentException = new NullAssignmentException();

            var expectedAssignmentValidationException =
                new AssignmentValidationException(nullAssignmentException);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(nullAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                createAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnCreateIfAssignmentIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidStuent = new Assignment
            {
                Label = invalidText,
                Content = invalidText
            };

            var invalidAssignmentException = new InvalidAssignmentException();

            invalidAssignmentException.AddData(
                key: nameof(Assignment.Id),
                values: "Id is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.Label),
                values: "Text is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.Content),
                values: "Text is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.Deadline),
                values: "Date is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.CreatedBy),
                values: "Id is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.UpdatedBy),
                values: "Id is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.CreatedDate),
                values: "Date is required");

            invalidAssignmentException.AddData(
                key: nameof(Assignment.UpdatedDate),
                values: "Date is required");

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(invalidStuent);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                createAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedAssignmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment invalidAssignment = randomAssignment;
            invalidAssignment.UpdatedBy = Guid.NewGuid();

            var invalidAssignmentInputException = new InvalidAssignmentException();

            invalidAssignmentInputException.AddData(
                key: nameof(Assignment.UpdatedBy),
                values: $"Id is not the same as {nameof(Assignment.CreatedBy)}");

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(invalidAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                createAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedAssignmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment invalidAssignment = randomAssignment;
            invalidAssignment.UpdatedDate = GetRandomDateTime();
            var invalidAssignmentException = new InvalidAssignmentException();

            invalidAssignmentException.AddData(
                key: nameof(Assignment.UpdatedDate),
                values: $"Date is not the same as {nameof(Assignment.CreatedDate)}");

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(invalidAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                createAssignmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedAssignmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(It.IsAny<Assignment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.UpdatedBy = inputAssignment.CreatedBy;
            inputAssignment.CreatedDate = dateTime.AddMinutes(minutes);
            inputAssignment.UpdatedDate = inputAssignment.CreatedDate;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.CreatedDate),
                parameterValue: inputAssignment.CreatedDate);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                createAssignmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

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
        public async void ShouldThrowValidationExceptionOnCreateWhenAssignmentAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment alreadyExistsAssignment = randomAssignment;
            alreadyExistsAssignment.UpdatedBy = alreadyExistsAssignment.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAssignmentException =
                new AlreadyExistsAssignmentException(duplicateKeyException);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(alreadyExistsAssignmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAsync(alreadyExistsAssignment))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Assignment> createAssignmentTask =
                this.assignmentService.CreateAssignmentAsync(alreadyExistsAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                createAssignmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAsync(alreadyExistsAssignment),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
