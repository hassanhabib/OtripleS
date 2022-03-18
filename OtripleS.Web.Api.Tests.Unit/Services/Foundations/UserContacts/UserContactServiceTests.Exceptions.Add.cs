// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact inputUserContact = randomUserContact;
            var sqlException = GetSqlException();

            var failedUserContactStorageException =
                new FailedUserContactStorageException(sqlException);

            var expectedUserContactDependencyException =
                new UserContactDependencyException(failedUserContactStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserContactAsync(inputUserContact))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserContact> addUserContactTask =
                this.userContactService.AddUserContactAsync(inputUserContact);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(inputUserContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact inputUserContact = randomUserContact;
            var databaseUpdateException = new DbUpdateException();

            var failedUserContactStorageException =
                new FailedUserContactStorageException(databaseUpdateException);

            var expectedUserContactDependencyException =
                new UserContactDependencyException(failedUserContactStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserContactAsync(inputUserContact))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<UserContact> addUserContactTask =
                this.userContactService.AddUserContactAsync(inputUserContact);

            // then
            await Assert.ThrowsAsync<UserContactDependencyException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(inputUserContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact inputUserContact = randomUserContact;
            var serviceException = new Exception();

            var failedUserContactServiceException =
                new FailedUserContactServiceException(serviceException);

            var expectedUserContactServiceException =
                new UserContactServiceException(failedUserContactServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserContactAsync(It.IsAny<UserContact>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<UserContact> addUserContactTask =
                 this.userContactService.AddUserContactAsync(inputUserContact);

            // then
            await Assert.ThrowsAsync<UserContactServiceException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserContactServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(It.IsAny<UserContact>()),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
