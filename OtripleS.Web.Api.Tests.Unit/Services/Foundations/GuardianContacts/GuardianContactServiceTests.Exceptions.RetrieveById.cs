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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var inputContactId = randomContactId;
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            SqlException sqlException = GetSqlException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianContactByIdAsync(inputGuardianId, inputContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(
                    inputGuardianId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(() =>
                retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuardianContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(inputGuardianId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputGuardianId = randomGuardianId;
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(inputGuardianId, inputContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync
                (inputGuardianId, inputContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(
                () => retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(inputGuardianId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputGuardianId = randomGuardianId;
            var serviceException = new Exception();

            var failedGuardianContactServiceException =
                new FailedGuardianContactServiceException(serviceException);

            var expectedGuardianContactServiceException =
                new GuardianContactServiceException(failedGuardianContactServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(inputGuardianId, inputContactId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(
                    inputGuardianId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactServiceException>(() =>
                retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianContactServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(inputGuardianId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
