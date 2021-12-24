//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfUserContactIsNullAndLogItAsync()
        {
            // given
            UserContact randomUserContact = null;
            UserContact nullUserContact = randomUserContact;
            var nullUserContactException = new NullUserContactException();

            var expectedUserContactValidationException =
                new UserContactValidationException(nullUserContactException);

            // when
            ValueTask<UserContact> addUserContactTask =
                this.userContactService.AddUserContactAsync(nullUserContact);

            // then
            await Assert.ThrowsAsync<UserContactValidationException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfUserIdOrContactIdInvalidAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact inputUserContact = new UserContact();

            var invalidUserContactException = new InvalidUserContactException();

            invalidUserContactException.AddData(
                key: nameof(UserContact.UserId),
                values: "Id is required");

            invalidUserContactException.AddData(
                key: nameof(UserContact.ContactId),
                values: "Id is required");

            var expectedUserContactValidationException =
                new UserContactValidationException(invalidUserContactException);

            // when
            ValueTask<UserContact> addUserContactTask =
                this.userContactService.AddUserContactAsync(inputUserContact);

            // then
            await Assert.ThrowsAsync<UserContactValidationException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedUserContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfUserContactAlreadyExistsAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact alreadyExistsUserContact = randomUserContact;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsUserContactException =
                new AlreadyExistsUserContactException(duplicateKeyException);

            var expectedUserContactValidationException =
                new UserContactValidationException(alreadyExistsUserContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserContactAsync(alreadyExistsUserContact))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<UserContact> addUserContactTask =
                this.userContactService.AddUserContactAsync(alreadyExistsUserContact);

            // then
            await Assert.ThrowsAsync<UserContactValidationException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedUserContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(alreadyExistsUserContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferneceExceptionAndLogItAsync()
        {
            // given
            UserContact randomUserContact = CreateRandomUserContact();
            UserContact invalidUserContact = randomUserContact;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidUserContactReferenceException =
                new InvalidUserContactReferenceException(foreignKeyConstraintConflictException);

            var expectedUserContactValidationException =
                new UserContactValidationException(invalidUserContactReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserContactAsync(invalidUserContact))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<UserContact> addUserContactTask =
                this.userContactService.AddUserContactAsync(invalidUserContact);

            // then
            await Assert.ThrowsAsync<UserContactValidationException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedUserContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(invalidUserContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
