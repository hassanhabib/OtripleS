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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var inputContactId = randomContactId;
            Guid randomUserId = Guid.NewGuid();
            Guid inputUserId = randomUserId;
            SqlException sqlException = GetSqlException();

            var expectedUserContactDependencyException =
                new UserContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectUserContactByIdAsync(inputUserId, inputContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserContact> retrieveUserContactTask =
                this.userContactService.RetrieveUserContactByIdAsync(
                    inputUserId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(() =>
                retrieveUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(inputUserId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomUserId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputUserId = randomUserId;
            var databaseUpdateException = new DbUpdateException();

            var expectedUserContactDependencyException =
                new UserContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(inputUserId, inputContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<UserContact> retrieveUserContactTask =
                this.userContactService.RetrieveUserContactByIdAsync
                (inputUserId, inputContactId);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(
                () => retrieveUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(inputUserId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomUserId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputUserId = randomUserId;
            var exception = new Exception();

            var expectedUserContactException =
                new UserContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(inputUserId, inputContactId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<UserContact> retrieveUserContactTask =
                this.userContactService.RetrieveUserContactByIdAsync(
                    inputUserId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<UserContactServiceException>(() =>
                retrieveUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(inputUserId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
