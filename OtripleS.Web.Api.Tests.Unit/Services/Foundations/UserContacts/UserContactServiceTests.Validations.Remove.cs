// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenUserIdOrContactIdIsInvalidAndLogItAsync()
        {
            // given
            Guid inputContactId = Guid.Empty;
            Guid inputUserId = Guid.Empty;

            var invalidUserContactException = new InvalidUserContactException();

            invalidUserContactException.AddData(
                key: nameof(UserContact.ContactId),
                values: "Id is required");

            invalidUserContactException.AddData(
                key: nameof(UserContact.UserId),
                values: "Id is required");

            var expectedUserContactValidationException =
                new UserContactValidationException(invalidUserContactException);

            // when
            ValueTask<UserContact> removeUserContactTask =
                this.userContactService.RemoveUserContactByIdAsync(inputUserId, inputContactId);

            // then
            await Assert.ThrowsAsync<UserContactValidationException>(() => removeUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedUserContactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageUserContactIsInvalidAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            Guid inputContactId = randomUserContact.ContactId;
            Guid inputUserId = randomUserContact.UserId;
            UserContact nullStorageUserContact = null;

            var notFoundUserContactException =
                new NotFoundUserContactException(inputUserId, inputContactId);

            var expectedSemesterCourseValidationException =
                new UserContactValidationException(notFoundUserContactException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectUserContactByIdAsync(inputUserId, inputContactId))
                    .ReturnsAsync(nullStorageUserContact);

            // when
            ValueTask<UserContact> removeUserContactTask =
                this.userContactService.RemoveUserContactByIdAsync(inputUserId, inputContactId);

            // then
            await Assert.ThrowsAsync<UserContactValidationException>(() =>
                removeUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
