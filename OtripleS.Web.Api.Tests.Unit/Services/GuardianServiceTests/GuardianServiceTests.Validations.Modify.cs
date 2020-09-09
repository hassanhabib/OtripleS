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
    }
}
