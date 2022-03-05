﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.UserContacts
{
    public partial class UserContactServiceTests
    {        

        [Fact]
        public async Task ShouldRemoveUserContactAsync()
        {
            // given
            var randomUserId = Guid.NewGuid();
            var randomContactId = Guid.NewGuid();
            Guid inputUserId = randomUserId;
            Guid inputContactId = randomContactId;
            UserContact randomUserContact = CreateRandomUserContact();
            randomUserContact.UserId = inputUserId;
            randomUserContact.ContactId = inputContactId;
            UserContact storageUserContact = randomUserContact;
            UserContact expectedUserContact = storageUserContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(inputUserId, inputContactId))
                    .ReturnsAsync(storageUserContact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteUserContactAsync(storageUserContact))
                    .ReturnsAsync(expectedUserContact);

            // when
            UserContact actualUserContact =
                await this.userContactService.RemoveUserContactByIdAsync(inputUserId, inputContactId);

            // then
            actualUserContact.Should().BeEquivalentTo(expectedUserContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(inputUserId, inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserContactAsync(storageUserContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldRetrieveUserContactById()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact storageUserContact = randomUserContact;
            UserContact expectedUserContact = storageUserContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserContactByIdAsync(randomUserContact.UserId, randomUserContact.ContactId))
                    .ReturnsAsync(randomUserContact);

            // when
            UserContact actualUserContact = await
                this.userContactService.RetrieveUserContactByIdAsync(
                    randomUserContact.UserId, randomUserContact.ContactId);

            // then
            actualUserContact.Should().BeEquivalentTo(expectedUserContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(randomUserContact.UserId, randomUserContact.ContactId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
