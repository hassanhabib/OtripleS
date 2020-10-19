// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Guardians
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenGuardianIsNullAndLogItAsync()
        {
            //given
            Guardian invalidGuardian = null;
            var nullGuardianException = new NullGuardianException();

            var expectedGuardianValidationException =
                new GuardianValidationException(nullGuardianException);

            //when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(invalidGuardian);

            //then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenGuardianIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidGuardianId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian invalidGuardian = randomGuardian;
            invalidGuardian.Id = invalidGuardianId;

            var invalidGuardianException = new InvalidGuardianException(
                parameterName: nameof(Guardian.Id),
                parameterValue: invalidGuardian.Id);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianException);

            //when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(invalidGuardian);

            //then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
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
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.CreatedBy = default;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.CreatedBy),
                parameterValue: inputGuardian.CreatedBy);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
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
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.UpdatedBy = default;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.UpdatedBy),
                parameterValue: inputGuardian.UpdatedBy);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
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
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.CreatedDate = default;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.CreatedDate),
                parameterValue: inputGuardian.CreatedDate);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
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
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.UpdatedDate = default;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.UpdatedDate),
                parameterValue: inputGuardian.UpdatedDate);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
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
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.UpdatedDate),
                parameterValue: inputGuardian.UpdatedDate);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
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
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.UpdatedBy = inputGuardian.CreatedBy;
            inputGuardian.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.UpdatedDate),
                parameterValue: inputGuardian.UpdatedDate);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGuardianDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian nonExistentGuardian = randomGuardian;
            nonExistentGuardian.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Guardian noGuardian = null;
            var notFoundGuardianException = new NotFoundGuardianException(nonExistentGuardian.Id);

            var expectedGuardianValidationException =
                new GuardianValidationException(notFoundGuardianException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(nonExistentGuardian.Id))
                    .ReturnsAsync(noGuardian);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(nonExistentGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(nonExistentGuardian.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
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
            Guardian randomGuardian = CreateRandomGuardian(randomDate);
            Guardian invalidGuardian = randomGuardian;
            invalidGuardian.UpdatedDate = randomDate;
            Guardian storageGuardian = randomGuardian.DeepClone();
            Guid guardianId = invalidGuardian.Id;
            invalidGuardian.CreatedDate = storageGuardian.CreatedDate.AddMinutes(randomNumber);

            var invalidGuardianException = new InvalidGuardianException(
                parameterName: nameof(Guardian.CreatedDate),
                parameterValue: invalidGuardian.CreatedDate);

            var expectedGuardianValidationException =
              new GuardianValidationException(invalidGuardianException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(guardianId))
                    .ReturnsAsync(storageGuardian);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(invalidGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(invalidGuardian.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
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
            Guardian randomGuardian = CreateRandomGuardian(randomDate);
            Guardian invalidGuardian = randomGuardian;
            invalidGuardian.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Guardian storageGuardian = randomGuardian.DeepClone();
            Guid guardianId = invalidGuardian.Id;
            invalidGuardian.CreatedBy = invalidCreatedBy;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.CreatedBy),
                parameterValue: invalidGuardian.CreatedBy);

            var expectedGuardianValidationException =
              new GuardianValidationException(invalidGuardianInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(guardianId))
                    .ReturnsAsync(storageGuardian);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(invalidGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(invalidGuardian.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
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
            Guardian randomGuardian = CreateRandomGuardian(randomDate);
            randomGuardian.CreatedDate = randomGuardian.CreatedDate.AddMinutes(minutesInThePast);
            Guardian invalidGuardian = randomGuardian;
            invalidGuardian.UpdatedDate = randomDate;
            Guardian storageGuardian = randomGuardian.DeepClone();
            Guid guardianId = invalidGuardian.Id;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.UpdatedDate),
                parameterValue: invalidGuardian.UpdatedDate);

            var expectedGuardianValidationException =
              new GuardianValidationException(invalidGuardianInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(guardianId))
                    .ReturnsAsync(storageGuardian);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(invalidGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                modifyGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(invalidGuardian.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
