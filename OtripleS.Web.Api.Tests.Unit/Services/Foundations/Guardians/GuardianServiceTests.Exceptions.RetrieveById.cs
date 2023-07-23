// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Guardians
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someGuardianId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedDependencyException =
                new GuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(someGuardianId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Guardian> retrieveGuardianTask =
                this.guardianService.RetrieveGuardianByIdAsync(someGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianDependencyException>(() =>
                retrieveGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(someGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someGuardianId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianDependencyException =
                new GuardianDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(someGuardianId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Guardian> retrieveGuardianTask =
                this.guardianService.RetrieveGuardianByIdAsync(someGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianDependencyException>(() =>
                retrieveGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(someGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someGuardianId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedGuardianServiceException =
                new FailedGuardianServiceException(serviceException);

            var expectedGuardianServiceException =
                new GuardianServiceException(failedGuardianServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(someGuardianId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Guardian> retrieveGuardianByIdTask =
                this.guardianService.RetrieveGuardianByIdAsync(someGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianServiceException>(() =>
                retrieveGuardianByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(someGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
