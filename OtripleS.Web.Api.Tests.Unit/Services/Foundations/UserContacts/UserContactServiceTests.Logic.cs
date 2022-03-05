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
        public void ShouldRetrieveAllUserContacts()
        {
            // given
            IQueryable<UserContact> randomUserContacts =
                CreateRandomUserContacts();

            IQueryable<UserContact> storageUserContacts = randomUserContacts;
            IQueryable<UserContact> expectedUserContacts = storageUserContacts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUserContacts())
                    .Returns(storageUserContacts);

            // when
            IQueryable<UserContact> actualUserContacts =
                this.userContactService.RetrieveAllUserContacts();

            // then
            actualUserContacts.Should().BeEquivalentTo(expectedUserContacts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUserContacts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }        
    }
}
