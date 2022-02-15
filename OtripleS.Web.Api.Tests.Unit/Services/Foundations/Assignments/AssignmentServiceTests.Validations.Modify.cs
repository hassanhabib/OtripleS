// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAssignmentIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidAssignment = new Assignment
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

            //when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            //then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment invalidAssignment = randomAssignment;
            var invalidAssignmentException = new InvalidAssignmentException();

            invalidAssignmentException.AddData(
                key: nameof(Assignment.UpdatedDate),
                values: $"UpdatedDate is the same as {nameof(Assignment.CreatedDate)}");

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

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

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;
            inputAssignment.UpdatedBy = inputAssignment.CreatedBy;
            inputAssignment.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidAssignmentException = new InvalidAssignmentException();

            invalidAssignmentException.AddData(
                key: nameof(Assignment.CreatedDate),
                values: $"Date is not recent");

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(inputAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnModifyIfAssignmentDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment nonExistentAssignment = randomAssignment;
            nonExistentAssignment.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Assignment noAssignment = null;
            var notFoundAssignmentException = new NotFoundAssignmentException(nonExistentAssignment.Id);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(notFoundAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(nonExistentAssignment.Id))
                    .ReturnsAsync(noAssignment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(nonExistentAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(nonExistentAssignment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(randomDate);
            Assignment invalidAssignment = randomAssignment;
            invalidAssignment.UpdatedDate = randomDate;
            Assignment storageAssignment = randomAssignment.DeepClone();
            Guid assignmentId = invalidAssignment.Id;
            invalidAssignment.CreatedDate = storageAssignment.CreatedDate.AddMinutes(randomNumber);

            var invalidAssignmentException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.CreatedDate),
                parameterValue: invalidAssignment.CreatedDate);

            var expectedAssignmentValidationException =
              new AssignmentValidationException(invalidAssignmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId))
                    .ReturnsAsync(storageAssignment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(invalidAssignment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(randomDate);
            randomAssignment.CreatedDate = randomAssignment.CreatedDate.AddMinutes(minutesInThePast);
            Assignment invalidAssignment = randomAssignment;
            invalidAssignment.UpdatedDate = randomDate;
            Assignment storageAssignment = randomAssignment.DeepClone();
            Guid assignmentId = invalidAssignment.Id;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.UpdatedDate),
                parameterValue: invalidAssignment.UpdatedDate);

            var expectedAssignmentValidationException =
              new AssignmentValidationException(invalidAssignmentInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId))
                    .ReturnsAsync(storageAssignment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(invalidAssignment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(randomDate);
            Assignment invalidAssignment = randomAssignment;
            invalidAssignment.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Assignment storageAssignment = randomAssignment.DeepClone();
            Guid assignmentId = invalidAssignment.Id;
            invalidAssignment.CreatedBy = invalidCreatedBy;

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.CreatedBy),
                parameterValue: invalidAssignment.CreatedBy);

            var expectedAssignmentValidationException =
              new AssignmentValidationException(invalidAssignmentInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentByIdAsync(assignmentId))
                    .ReturnsAsync(storageAssignment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Assignment> modifyAssignmentTask =
                this.assignmentService.ModifyAssignmentAsync(invalidAssignment);

            // then
            await Assert.ThrowsAsync<AssignmentValidationException>(() =>
                modifyAssignmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentByIdAsync(invalidAssignment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
