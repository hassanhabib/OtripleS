// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Guardians
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenIdIsNullAndLogItAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Guid randomGuardianId = default;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianException = new InvalidGuardianException(
                parameterName: nameof(Guardian.Id),
                parameterValue: inputGuardianId);

            var expectedValidationException =
                new GuardianValidationException(invalidGuardianException);

            // when
            ValueTask<Guardian> retrieveGuardianTask =
                this.guardianService.RetrieveGuardianByIdAsync(inputGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                retrieveGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageGuardianIsNullAndLogItAsync()
        {
            // given
            DateTimeOffset dateTimeOffset = GetRandomDateTime();
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            Guardian nullGuardian = default;
            var nullGuardianException = new NotFoundGuardianException(guardianId: inputGuardianId);

            var expectedValidationException =
                new GuardianValidationException(nullGuardianException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId))
                    .ReturnsAsync(nullGuardian);

            // when
            ValueTask<Guardian> retrieveGuardianTask =
                this.guardianService.RetrieveGuardianByIdAsync(inputGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                retrieveGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
