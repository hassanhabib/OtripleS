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
        public async Task ShouldModifyUserAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            User randomUser = CreateRandomUser(dates: randomInputDate);
            User inputUser = randomUser;
            User afterUpdateStorageUser = inputUser;
            User expectedUser = afterUpdateStorageUser;
            User beforeUpdateStorageUser = randomUser.DeepClone();
            inputUser.UpdatedDate = randomDate;
            Guid userId = inputUser.Id;

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ReturnsAsync(beforeUpdateStorageUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(inputUser))
                    .ReturnsAsync(afterUpdateStorageUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            User actualUser =
                await this.userService.ModifyUserAsync(inputUser);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(inputUser),
                    Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
 