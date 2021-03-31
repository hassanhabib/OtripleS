// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUserIsNullAndLogItAsync()
        {
            // given
            User randomUser = null;
            User nullUser = randomUser;
            var nullUserException = new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(nullUserException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(nullUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenIdIsInvalidAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            inputUser.Id = default;

            var invalidUserInputException = new InvalidUserException(
                parameterName: nameof(User.Id),
                parameterValue: inputUser.Id);

            var expectedUserValidationException =
                new UserValidationException(invalidUserInputException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenUserUserNameIsInvalidAndLogItAsync(
            string invalidUserUserName)
        {
            // given
            User randomUser = CreateRandomUser();
            User invalidUser = randomUser;
            invalidUser.UserName = invalidUserUserName;

            var invalidUserException = new InvalidUserException(
               parameterName: nameof(User.UserName),
               parameterValue: invalidUser.UserName);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenUsersNameIsInvalidAndLogItAsync(
            string invalidName)
        {
            // given
            User randomUser = CreateRandomUser();
            User invalidUser = randomUser;
            invalidUser.Name = invalidName;

            var invalidUserException = new InvalidUserException(
               parameterName: nameof(User.Name),
               parameterValue: invalidUser.Name);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenUserFamilyNameIsInvalidAndLogItAsync(
            string invalidUserFamilyName)
        {
            // given
            User randomUser = CreateRandomUser();
            User invalidUser = randomUser;
            invalidUser.FamilyName = invalidUserFamilyName;

            var invalidUserException = new InvalidUserException(
               parameterName: nameof(User.FamilyName),
               parameterValue: invalidUser.FamilyName);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            inputUser.UpdatedDate = default;

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.UpdatedDate),
                parameterValue: inputUser.UpdatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            inputUser.CreatedDate = default;

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.CreatedDate),
                parameterValue: inputUser.CreatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            User randomUser = CreateRandomUser(dateTime);
            User inputUser = randomUser;

            var invalidUserInputException = new InvalidUserException(
                parameterName: nameof(User.UpdatedDate),
                parameterValue: inputUser.UpdatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserInputException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            User randomUser = CreateRandomUser(dateTime);
            User inputUser = randomUser;
            inputUser.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidUserInputException = new InvalidUserException(
                parameterName: nameof(User.UpdatedDate),
                parameterValue: inputUser.UpdatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            User randomUser = CreateRandomUser(dateTime);
            User nonExistentUser = randomUser;
            nonExistentUser.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            User noUser = null;
            var notFoundUserException = new NotFoundUserException(nonExistentUser.Id);

            var expectedUserValidationException =
                new UserValidationException(notFoundUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(nonExistentUser.Id))
                    .ReturnsAsync(noUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(nonExistentUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(nonExistentUser.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            User randomUser = CreateRandomUser(randomDate);
            User invalidUser = randomUser;
            invalidUser.UpdatedDate = randomDate;
            User storageUser = randomUser.DeepClone();
            Guid UserId = invalidUser.Id;
            invalidUser.CreatedDate = storageUser.CreatedDate.AddMinutes(randomNumber);

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.CreatedDate),
                parameterValue: invalidUser.CreatedDate);

            var expectedUserValidationException =
              new UserValidationException(invalidUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(UserId))
                    .ReturnsAsync(storageUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(invalidUser.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
