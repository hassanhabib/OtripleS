// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserServiceTests
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomUserId = Guid.NewGuid();
            Guid inputUserId = randomUserId;
            SqlException sqlException = GetSqlException();

            var expectedUserDependencyException =
                new UserDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<User> deleteUserTask =
                this.userService.DeleteUserAsync(inputUserId);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                deleteUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
