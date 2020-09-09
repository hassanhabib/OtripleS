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

    }
}
