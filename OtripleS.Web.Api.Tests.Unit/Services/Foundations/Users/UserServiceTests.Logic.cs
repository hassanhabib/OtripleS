﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Users;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
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

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(inputUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.DeleteUserAsync(inputUser))
                    .ReturnsAsync(storageUser);

            // when
            User actualUser =
                await this.userService.RemoveUserByIdAsync(inputUserId);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(inputUser),
                    Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllUsers()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<User> randomUsers = CreateRandomUsers(dates: randomDateTime);
            IQueryable<User> storageUsers = randomUsers;
            IQueryable<User> expectedUsers = storageUsers;

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
                    .Returns(storageUsers);

            // when
            IQueryable<User> actualUsers =
                this.userService.RetrieveAllUsers();

            // then
            actualUsers.Should().BeEquivalentTo(expectedUsers);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

 