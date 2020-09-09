// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Guardian;
using Xunit;
using OtripleS.Web.Api.Models.Guardian.Exceptions;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianServiceTests
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
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
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
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenGuardianFirstNameIsInvalidAndLogItAsync(
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

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnCreateWhenGuardianFamilyNameIsInvalidAndLogItAsync(
            string invalidGuardianFirstName)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(dateTime);
            Guardian invalidGuardian = randomGuardian;
            invalidGuardian.FirstName = invalidGuardianFirstName;

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

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
