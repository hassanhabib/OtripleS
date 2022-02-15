// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = inputRegistration.CreatedBy;
            var sqlException = GetSqlException();

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRegistrationAsync(inputRegistration))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Registration> createRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                createRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(inputRegistration),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = inputRegistration.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRegistrationAsync(inputRegistration))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Registration> createRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                createRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(inputRegistration),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = inputRegistration.CreatedBy;
            var serviceException = new Exception();

            var expectedRegistrationServiceException =
                new RegistrationServiceException(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRegistrationAsync(inputRegistration))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Registration> createRegistrationTask =
                 this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationServiceException>(() =>
                createRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(inputRegistration),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


    }
}
