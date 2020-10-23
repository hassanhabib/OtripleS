// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Guardians
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            var sqlException = GetSqlException();

            var expectedDependencyException =
                new GuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Guardian> retrieveGuardianTask =
                this.guardianService.RetrieveGuardianByIdAsync(inputGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianDependencyException>(() =>
                retrieveGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianDependencyException =
                new GuardianDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Guardian> retrieveGuardianTask =
                this.guardianService.RetrieveGuardianByIdAsync(inputGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianDependencyException>(() =>
                retrieveGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            var exception = new Exception();

            var expectedGuardianServiceException =
                new GuardianServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Guardian> retrieveGuardianByIdTask =
                this.guardianService.RetrieveGuardianByIdAsync(inputGuardianId);

            // then
            await Assert.ThrowsAsync<GuardianServiceException>(() =>
                retrieveGuardianByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(inputGuardianId),
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
