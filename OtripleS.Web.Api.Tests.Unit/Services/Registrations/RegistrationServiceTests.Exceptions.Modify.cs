// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Force.DeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration someRegistration = CreateRandomRegistration(dateTime: randomDateTime);
            SqlException sqlException = GetSqlException();
            var expectedRegistrationDependencyException = new RegistrationDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(someRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedRegistrationDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration storageRegistration = randomRegistration;
            Registration foreignKeyConflictedRegistration = storageRegistration.DeepClone();
            storageRegistration.UpdatedDate = dateTime.AddMinutes(GetNegativeRandomNumber());
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedRegistrationException =
                new LockedRegistrationException(dbUpdateConcurrencyException);

            var registrationValidationException =
                new RegistrationDependencyException(lockedRegistrationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(foreignKeyConflictedRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(registrationValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
