﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidRegistrationId = Guid.Empty;

            var invalidRegistrationException = new InvalidRegistrationException(
                parameterName: nameof(Registration.Id),
                parameterValue: invalidRegistrationId);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationException);

            //when
            ValueTask<Registration> retrieveRegistrationByIdTask =
                this.registrationService.RetrieveRegistrationByIdAsync(invalidRegistrationId);

            //then
            await Assert.ThrowsAsync<RegistrationValidationException>(() => retrieveRegistrationByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageRegistrationIsNullAndLogItAsync()
        {
            //given
            Guid invalidRegistrationId = Guid.NewGuid();
            Registration invalidStorageRegistration = null;
            var notFoundRegistrationException = new NotFoundRegistrationException(invalidRegistrationId);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(notFoundRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageRegistration);

            //when
            ValueTask<Registration> retrieveRegistrationByIdTask =
                this.registrationService.RetrieveRegistrationByIdAsync(invalidRegistrationId);

            //then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                retrieveRegistrationByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
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
