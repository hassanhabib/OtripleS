// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRegistrationId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Registration> retrieveRegistrationTask =
                this.registrationService.RetrieveRegistrationByIdAsync(someRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                retrieveRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRegistrationId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Registration> retrieveByIdRegistrationTask =
                this.registrationService.RetrieveRegistrationByIdAsync(someRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                retrieveByIdRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRegistrationId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedRegistrationException =
                new LockedRegistrationException(databaseUpdateConcurrencyException);

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(lockedRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Registration> retrieveByIdRegistrationTask =
                this.registrationService.RetrieveRegistrationByIdAsync(someRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                retrieveByIdRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRegistrationId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedRegistrationServiceException =
                new FailedRegistrationServiceException(serviceException);

            var expectedRegistrationServiceException =
                new RegistrationServiceException(failedRegistrationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Registration> retrieveByIdRegistrationTask =
                this.registrationService.RetrieveRegistrationByIdAsync(someRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationServiceException>(() =>
                retrieveByIdRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
