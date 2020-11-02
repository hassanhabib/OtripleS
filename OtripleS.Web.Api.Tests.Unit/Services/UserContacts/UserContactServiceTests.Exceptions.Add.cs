//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact inputUserContact = randomUserContact;
            var sqlException = GetSqlException();

            var expectedUserContactDependencyException =
                new UserContactDependencyException(sqlException);

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
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
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

            var expectedUserContactDependencyException =
                new UserContactDependencyException(databaseUpdateException);

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
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
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
            var exception = new Exception();

            var expectedUserContactServiceException =
                new UserContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserContactAsync(inputUserContact))
                    .ThrowsAsync(exception);

            // when
            ValueTask<UserContact> addUserContactTask =
                 this.userContactService.AddUserContactAsync(inputUserContact);

            // then
            await Assert.ThrowsAsync<UserContactServiceException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(inputUserContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
