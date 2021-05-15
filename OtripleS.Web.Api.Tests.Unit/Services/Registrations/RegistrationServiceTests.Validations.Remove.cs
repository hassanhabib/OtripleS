// ---------------------------------------------------------------
// Copyright (c) PiorSoft, LLC. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidTaskCategotyId = Guid.Empty;

            var invalidRegistrationException = new InvalidRegistrationException(
                parameterName: nameof(Registration.Id),
                parameterValue: invalidTaskCategotyId);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationException);

            // when
            ValueTask<Registration> deleteRegistrationTask =
                this.registrationService.RemoveRegistrationByIdAsync(invalidTaskCategotyId);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                deleteRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls(); 
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfRegistrationIsNotFoundAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime: randomDateTime);
            Guid actualRegistrationId = randomRegistration.Id;
            Registration noRegistration = null;

            var notFoundRegistrationException =
                new NotFoundRegistrationException(randomRegistration.Id);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(notFoundRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(actualRegistrationId)).
                    ReturnsAsync(noRegistration);

            // when
            ValueTask<Registration> removeRegistrationByIdTask =
                this.registrationService.RemoveRegistrationByIdAsync(actualRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                removeRegistrationByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();            
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }        
    }
}
