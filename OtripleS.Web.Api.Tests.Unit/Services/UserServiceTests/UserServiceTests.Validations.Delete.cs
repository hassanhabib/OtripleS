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

namespace OtripleS.Web.Api.Tests.Unit.Services.UserServiceTests
{
	public partial class UserServiceTests
	{
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenUserIdIsInvalidAndLogItAsync()
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
                this.userService.DeleteUserAsync(inputUserId);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() => actualUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageUserIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            User randomUser = CreateRandomUser(dateTime);
            Guid inputUserId = randomUser.Id;
            User inputUser = randomUser;
            User nullStorageUser = null;

            var notFoundUserException = new NotFoundUserException(inputUserId);

            var expectedUserValidationException =
                new UserValidationException(notFoundUserException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(nullStorageUser);

            // when
            ValueTask<User> actualUserTask =
                this.userService.DeleteUserAsync(inputUserId);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() => actualUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
