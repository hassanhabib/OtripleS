using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenRegistrationIsNullAndLogItAsync()
        {
            // given
            Registration randomRegistration = default;
            Registration nullRegistration = randomRegistration;
            var nullRegistrationException = new NullRegistrationException();

            var expectedRegistrationValidationException =
                new RegistrationValidationException(nullRegistrationException);

            // when
            ValueTask<Registration> createRegistrationTask =
                this.registrationService.AddRegistrationAsync(nullRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                createRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
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
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.Id = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.Id),
                parameterValue: inputRegistration.Id);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> createRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                createRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRegisterWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.CreatedBy = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.CreatedBy),
                parameterValue: inputRegistration.CreatedBy);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRegisterWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.UpdatedBy),
                parameterValue: inputRegistration.UpdatedBy);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
