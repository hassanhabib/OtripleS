//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someGuardianId = randomGuardianId;
            SqlException sqlException = GetSqlException();

            var expectedGuardianContactDependencyException
                = new GuardianContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianContact> removeGuardianContactTask =
                this.guardianContactService.RemoveGuardianContactByIdAsync(
                    someGuardianId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(() =>
                removeGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuardianContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianContactAsync(It.IsAny<GuardianContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someGuardianId = randomGuardianId;
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianContact> removeGuardianContactTask =
                this.guardianContactService.RemoveGuardianContactByIdAsync
                (someGuardianId, someContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(
                () => removeGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianContactAsync(It.IsAny<GuardianContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someGuardianId = randomGuardianId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedContactException =
                new LockedGuardianContactException(databaseUpdateConcurrencyException);

            var expectedGuardianContactException =
                new GuardianContactDependencyException(lockedContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<GuardianContact> removeGuardianContactTask =
                this.guardianContactService.RemoveGuardianContactByIdAsync(someGuardianId, someContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(() =>
                removeGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianContactAsync(It.IsAny<GuardianContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someGuardianId = randomGuardianId;
            var exception = new Exception();

            var expectedGuardianContactException =
                new GuardianContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<GuardianContact> removeGuardianContactTask =
                this.guardianContactService.RemoveGuardianContactByIdAsync(
                    someGuardianId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactServiceException>(() =>
                removeGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(someGuardianId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianContactAsync(It.IsAny<GuardianContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
