// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var someContactId = Guid.NewGuid();
            Guid someUserId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedUserContactStorageException =
                new FailedUserContactStorageException(sqlException);

            var expectedUserContactDependencyException =
                new UserContactDependencyException(failedUserContactStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectUserContactByIdAsync(someUserId, someContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserContact> retrieveUserContactTask =
                this.userContactService.RetrieveUserContactByIdAsync(
                    someUserId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(() =>
                retrieveUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someUserId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedUserContactStorageException =
                new FailedUserContactStorageException(databaseUpdateException);

            var expectedUserContactDependencyException =
                new UserContactDependencyException(failedUserContactStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<UserContact> retrieveUserContactTask =
                this.userContactService.RetrieveUserContactByIdAsync(someUserId, someContactId);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(
                () => retrieveUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(someUserId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someUserId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedUserContactServiceException =
                new FailedUserContactServiceException(serviceException);

            var expectedUserContactException =
                new UserContactServiceException(failedUserContactServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<UserContact> retrieveUserContactTask =
                this.userContactService.RetrieveUserContactByIdAsync(someUserId, someContactId);

            // then
            await Assert.ThrowsAsync<UserContactServiceException>(() =>
                retrieveUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserContactException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
