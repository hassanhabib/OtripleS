// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Guardians
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianIsNullAndLogItAsync()
        {
            // given
            Guardian randomGuardian = default;
            Guardian nullGuardian = randomGuardian;
            var nullGuardianException = new NullGuardianException();

            var expectedGuardianValidationException =
                new GuardianValidationException(nullGuardianException);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(nullGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.Id = default;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.Id),
                parameterValue: inputGuardian.Id);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenGuardianFirstNameIsInvalidAndLogItAsync(
            string invalidGuardianFirstName)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian invalidGuardian = randomGuardian;
            invalidGuardian.FirstName = invalidGuardianFirstName;

            var invalidGuardianException = new InvalidGuardianException(
               parameterName: nameof(Guardian.FirstName),
               parameterValue: invalidGuardian.FirstName);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianException);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(invalidGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenGuardianFamilyNameIsInvalidAndLogItAsync(
            string invalidGuardianFirstName)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian invalidGuardian = randomGuardian;
            invalidGuardian.FamilyName = invalidGuardianFirstName;

            var invalidGuardianException = new InvalidGuardianException(
               parameterName: nameof(Guardian.FamilyName),
               parameterValue: invalidGuardian.FamilyName);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianException);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(invalidGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                 broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                     Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.UpdatedBy = Guid.NewGuid();

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.UpdatedBy),
                parameterValue: inputGuardian.UpdatedBy);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.UpdatedBy = randomGuardian.CreatedBy;
            inputGuardian.UpdatedDate = GetRandomDateTime();

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.UpdatedDate),
                parameterValue: inputGuardian.UpdatedDate);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian inputGuardian = randomGuardian;
            inputGuardian.UpdatedBy = inputGuardian.CreatedBy;
            inputGuardian.CreatedDate = dateTime.AddMinutes(minutes);
            inputGuardian.UpdatedDate = inputGuardian.CreatedDate;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.CreatedDate),
                parameterValue: inputGuardian.CreatedDate);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(inputGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian alreadyExistsGuardian = randomGuardian;
            alreadyExistsGuardian.UpdatedBy = alreadyExistsGuardian.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsGuardianException =
                new AlreadyExistsGuardianException(duplicateKeyException);

            var expectedGuardianValidationException =
                new GuardianValidationException(alreadyExistsGuardianException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAsync(alreadyExistsGuardian))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Guardian> createGuardianTask =
                this.guardianService.CreateGuardianAsync(alreadyExistsGuardian);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                createGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAsync(alreadyExistsGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
