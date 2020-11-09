// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomUserId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someUserId = randomUserId;
            SqlException sqlException = GetSqlException();

            var expectedUserContactDependencyException
                = new UserContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectUserContactByIdAsync(someUserId, someContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserContact> removeUserContactTask =
                this.userContactService.RemoveUserContactByIdAsync(
                    someUserId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(() =>
                removeUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomUserId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someUserId = randomUserId;
            var databaseUpdateException = new DbUpdateException();

            var expectedUserContactDependencyException =
                new UserContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<UserContact> removeUserContactTask =
                this.userContactService.RemoveUserContactByIdAsync
                (someUserId, someContactId);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(
                () => removeUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomUserId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someUserId = randomUserId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedContactException =
                new LockedUserContactException(databaseUpdateConcurrencyException);

            var expectedUserContactException =
                new UserContactDependencyException(lockedContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<UserContact> removeUserContactTask =
                this.userContactService.RemoveUserContactByIdAsync(someUserId, someContactId);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(() =>
                removeUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomUserId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someUserId = randomUserId;
            var exception = new Exception();

            var expectedUserContactException =
                new UserContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<UserContact> removeUserContactTask =
                this.userContactService.RemoveUserContactByIdAsync(
                    someUserId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<UserContactServiceException>(() =>
                removeUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
