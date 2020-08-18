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
        public async Task ShouldCreateUserAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            User randomUser = CreateRandomUser(randomDateTime);
            User inputUser = randomUser;
            User storageUser = randomUser;
            User expectedUser = storageUser;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser, GetRandomPassword()))
                    .ReturnsAsync(storageUser);

            // when
            User actualUser =
                await this.userService.RegisterUserAsync(inputUser, GetRandomPassword());

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser, GetRandomPassword()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
