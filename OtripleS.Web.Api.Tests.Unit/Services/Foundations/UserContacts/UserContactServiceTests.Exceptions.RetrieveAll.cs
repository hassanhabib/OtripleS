// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedUserContactStorageException =
                new FailedUserContactStorageException(sqlException);

            var expectedUserContactDependencyException =
                new UserContactDependencyException(failedUserContactStorageException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllUserContacts())
                .Throws(sqlException);

            // when
            Action retrieveAllUserContactsAction = () =>
                this.userContactService.RetrieveAllUserContacts();

            // then
            Assert.Throws<UserContactDependencyException>(
                retrieveAllUserContactsAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(
                        expectedUserContactDependencyException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllUserContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedUserContactServiceException =
                new UserContactServiceException(serviceException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllUserContacts())
                .Throws(serviceException);

            // when
            Action retrieveAllUserContactsAction = () =>
                this.userContactService.RetrieveAllUserContacts();

            // then
            Assert.Throws<UserContactServiceException>(
                retrieveAllUserContactsAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedUserContactServiceException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllUserContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
