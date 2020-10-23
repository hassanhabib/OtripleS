// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUserIsNullAndLogItAsync()
        {
            // given
            User randomUser = null;
            User nullUser = randomUser;
            var nullUserException = new NullUserException();
            string password = GetRandomPassword();

            var expectedUserValidationException =
                new UserValidationException(nullUserException);

            // when
            ValueTask<User> createUserTask =
                this.userService.RegisterUserAsync(nullUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                createUserTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenIdIsInvalidAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            inputUser.Id = default;
            string password = GetRandomPassword();

            var invalidUserInputException = new InvalidUserException(
                parameterName: nameof(User.Id),
                parameterValue: inputUser.Id);

            var expectedUserValidationException =
                new UserValidationException(invalidUserInputException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnCreateWhenUserUserNameIsInvalidAndLogItAsync(
            string invalidUserUserName)
        {
            // given
            User randomUser = CreateRandomUser();
            User invalidUser = randomUser;
            invalidUser.UserName = invalidUserUserName;
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException(
               parameterName: nameof(User.UserName),
               parameterValue: invalidUser.UserName);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(invalidUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnCreateWhenUsersNameIsInvalidAndLogItAsync(
            string invalidUsersName)
        {
            // given
            User randomUser = CreateRandomUser();
            User invalidUser = randomUser;
            invalidUser.Name = invalidUsersName;
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException(
               parameterName: nameof(User.Name),
               parameterValue: invalidUser.Name);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(invalidUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnCreateWhenUserFamilyNameIsInvalidAndLogItAsync(
            string invalidUserFamilyName)
        {
            // given
            User randomUser = CreateRandomUser();
            User invalidUser = randomUser;
            invalidUser.FamilyName = invalidUserFamilyName;
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException(
               parameterName: nameof(User.FamilyName),
               parameterValue: invalidUser.FamilyName);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(invalidUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            inputUser.CreatedDate = default;
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.CreatedDate),
                parameterValue: inputUser.CreatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            inputUser.UpdatedDate = default;
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.UpdatedDate),
                parameterValue: inputUser.UpdatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            inputUser.UpdatedDate = GetRandomDateTime();
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.UpdatedDate),
                parameterValue: inputUser.UpdatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

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
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            User randomUser = CreateRandomUser(dates: dateTime);
            User inputUser = randomUser;
            inputUser.CreatedDate = dateTime.AddMinutes(minutes);
            inputUser.UpdatedDate = inputUser.CreatedDate;
            string password = GetRandomPassword();

            var invalidUserException = new InvalidUserException(
                parameterName: nameof(User.CreatedDate),
                parameterValue: inputUser.CreatedDate);

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUserAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            User randomUser = CreateRandomUser(dateTime);
            User alreadyExistsUser = randomUser;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);
            string password = GetRandomPassword();

            var alreadyExistsUserException =
                new AlreadyExistsUserException(duplicateKeyException);

            var expectedUserValidationException =
                new UserValidationException(alreadyExistsUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(alreadyExistsUser, password))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(alreadyExistsUser, password);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(alreadyExistsUser, password),
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
