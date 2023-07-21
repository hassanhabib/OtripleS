// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(
                        someGuardianId, someContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(() =>
                retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuardianContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(
                        someGuardianId, someContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(
                () => retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedGuardianContactServiceException =
                new FailedGuardianContactServiceException(serviceException);

            var expectedGuardianContactServiceException =
                new GuardianContactServiceException(failedGuardianContactServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(
                    someGuardianId, someContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactServiceException>(() =>
                retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianContactServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
