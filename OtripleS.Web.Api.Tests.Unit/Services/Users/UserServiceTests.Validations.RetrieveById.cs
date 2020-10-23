// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenUserIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomUserId = default;
            Guid inputUserId = randomUserId;

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.Id),
                parameterValue: inputUserId);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> actualUserTask =
                this.userService.RetrieveUserByIdAsync(inputUserId);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() => actualUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenStorageUserIsInvalidAndLogItAsync()
        {
            // given
            Guid randomUserId = Guid.NewGuid();
            Guid inputUserId = randomUserId;
            User invalidStorageUser = null;
            var notFoundUserException = new NotFoundUserException(inputUserId);

            var expectedUserValidationException =
                new UserValidationException(notFoundUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(invalidStorageUser);

            // when
            ValueTask<User> retrieveUserTask =
                this.userService.RetrieveUserByIdAsync(inputUserId);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                retrieveUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
