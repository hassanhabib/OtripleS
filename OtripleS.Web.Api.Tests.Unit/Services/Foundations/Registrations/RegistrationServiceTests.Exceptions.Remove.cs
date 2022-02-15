// ---------------------------------------------------------------
// Copyright (c) PiorSoft, LLC. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration someRegistration = CreateRandomRegistration(dateTime: randomDateTime);
            Guid inputRegistrationId = someRegistration.Id;
            Registration storageRegistration = someRegistration;
            SqlException sqlException = GetSqlException();
            var expectedException = new RegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Registration> deleteRegistrationTask =
                this.registrationService.RemoveRegistrationByIdAsync(inputRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                 deleteRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration someRegistration = CreateRandomRegistration(dateTime: randomDateTime);
            Guid someRegistrationId = someRegistration.Id;
            Registration storageRegistration = someRegistration;
            var dbUpdateException = new DbUpdateException();

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(dbUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()))
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<Registration> deleteRegistrationTask =
                this.registrationService.RemoveRegistrationByIdAsync(someRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                deleteRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDbConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration someRegistration = CreateRandomRegistration(dateTime: randomDateTime);
            Registration storageRegistration = someRegistration;
            Guid inputRegistrationId = storageRegistration.Id;

            var databaseConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedRegistrationException =
                new LockedRegistrationException(databaseConcurrencyException);

            var expectedRegistrationValidationException =
                new RegistrationDependencyException(lockedRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()))
                    .ThrowsAsync(databaseConcurrencyException);

            // when
            ValueTask<Registration> removeRegistrationByIdTask =
                this.registrationService.RemoveRegistrationByIdAsync(inputRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                removeRegistrationByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration someRegistration = CreateRandomRegistration(dateTime: randomDateTime);
            Guid someRegistrationId = someRegistration.Id;
            Registration storageRegistration = someRegistration;
            var serviceException = new Exception();
            var expectedServiceException = new RegistrationServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Registration> deleteRegistrationTask =
                this.registrationService.RemoveRegistrationByIdAsync(someRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationServiceException>(() =>
                deleteRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteRegistrationAsync(It.IsAny<Registration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
