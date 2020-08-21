// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Users;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserServiceTests
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldRegisterUserAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            User randomUser = CreateRandomUser(dates: dateTime);
            User inputUser = randomUser;
            User storageUser = randomUser;
            User expectedUser = storageUser;
            string password = GetRandomPassword();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser, password))
                    .ReturnsAsync(storageUser);

            // when
            User actualUser =
                await this.userService.RegisterUserAsync(inputUser, password);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser, password),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteUserAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            User randomUser = CreateRandomUser(dates: dateTime);
            Guid inputUserId = randomUser.Id;
            User inputUser = randomUser;
            User storageUser = randomUser;
            User expectedUser = randomUser;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(inputUser);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteUserAsync(inputUser))
                    .ReturnsAsync(storageUser);

            // when
            User actualUser =
                await this.userService.DeleteUserAsync(inputUserId);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(inputUser),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveUserById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            User randomUser = CreateRandomUser(dates: dateTime);
            Guid inputUserId = randomUser.Id;
            User inputUser = randomUser;
            User expectedUser = randomUser;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectUserByIdAsync(inputUserId))
                .ReturnsAsync(inputUser);

            //when 
            User actualUser = await this.userService.RetrieveUserById(inputUserId);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
