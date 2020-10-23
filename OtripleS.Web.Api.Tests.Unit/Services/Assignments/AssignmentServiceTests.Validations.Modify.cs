// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Assignments
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Assignment randomAssignment = CreateRandomAssignment(dateTime);
            Assignment inputAssignment = randomAssignment;

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

            var invalidAssignmentInputException = new InvalidAssignmentException(
                parameterName: nameof(Assignment.UpdatedDate),
                parameterValue: inputAssignment.UpdatedDate);

            var expectedAssignmentValidationException =
                new AssignmentValidationException(invalidAssignmentInputException);

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
