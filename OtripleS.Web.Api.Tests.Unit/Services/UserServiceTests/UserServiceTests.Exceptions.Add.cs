// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Users;
using Xunit;
using OtripleS.Web.Api.Models.Users.Exceptions;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserServiceTests
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            var sqlException = GetSqlException();
            string password = GetRandomPassword();

            var expectedUserDependencyException =
                new UserDependencyException(sqlException);


            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser, password))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                registerUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser, password),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
