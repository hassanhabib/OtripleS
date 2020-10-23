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
        public async void ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomGuardianId = default;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianInputException = new InvalidGuardianException(
                parameterName: nameof(Guardian.Id),
                parameterValue: inputGuardianId);

            var expectedGuardianValidationException =
                new GuardianValidationException(invalidGuardianInputException);

            // when
            ValueTask<Guardian> deleteGuardianTask =
                this.guardianService.DeleteGuardianByIdAsync(inputGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                deleteGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnDeleteWhenStorageGuardianIsNullAndLogItAsync()
        {
            // given
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            Guardian invalidStorageGuardian = null;
            var notFoundGuardianException = new NotFoundGuardianException(inputGuardianId);

            var expectedGuardianValidationException =
                new GuardianValidationException(notFoundGuardianException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId))
                    .ReturnsAsync(invalidStorageGuardian);

            // when
            ValueTask<Guardian> deleteGuardianTask =
                this.guardianService.DeleteGuardianByIdAsync(inputGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianValidationException>(() =>
                deleteGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAsync(It.IsAny<Guardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
