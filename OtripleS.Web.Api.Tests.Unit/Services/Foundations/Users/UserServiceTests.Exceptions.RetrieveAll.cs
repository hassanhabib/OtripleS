// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.Users.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
                    .Throws(sqlException);

         
            // when
            Action retrieveAllUsersAction = () =>
                this.userService.RetrieveAllUsers();

            // then
            Assert.Throws<UserServiceException>(
                retrieveAllUsersAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedUserServiceException =
                new UserServiceException(exception);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
                    .Throws(exception);

            

            // when
            Action retrieveAllUsersAction = () =>
                this.userService.RetrieveAllUsers();

            // then
            Assert.Throws<UserServiceException>(
                retrieveAllUsersAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserServiceException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
