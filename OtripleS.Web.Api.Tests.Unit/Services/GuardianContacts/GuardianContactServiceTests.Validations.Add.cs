//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianContactIsNullAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = default;
            GuardianContact nullGuardianContact = randomGuardianContact;
            var nullGuardianContactException = new NullGuardianContactException();

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(nullGuardianContactException);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                this.guardianContactService.AddGuardianContactAsync(nullGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(It.IsAny<GuardianContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact inputGuardianContact = randomGuardianContact;
            inputGuardianContact.GuardianId = default;

            var invalidGuardianContactInputException = new InvalidGuardianContactInputException(
                parameterName: nameof(GuardianContact.GuardianId),
                parameterValue: inputGuardianContact.GuardianId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(invalidGuardianContactInputException);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                this.guardianContactService.AddGuardianContactAsync(inputGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(It.IsAny<GuardianContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenContactIdIsInvalidAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact inputGuardianContact = randomGuardianContact;
            inputGuardianContact.ContactId = default;

            var invalidGuardianContactInputException = new InvalidGuardianContactInputException(
                parameterName: nameof(GuardianContact.ContactId),
                parameterValue: inputGuardianContact.ContactId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(invalidGuardianContactInputException);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                this.guardianContactService.AddGuardianContactAsync(inputGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(It.IsAny<GuardianContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenGuardianContactAlreadyExistsAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact alreadyExistsGuardianContact = randomGuardianContact;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsGuardianContactException =
                new AlreadyExistsGuardianContactException(duplicateKeyException);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(alreadyExistsGuardianContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianContactAsync(alreadyExistsGuardianContact))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                this.guardianContactService.AddGuardianContactAsync(alreadyExistsGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(alreadyExistsGuardianContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact invalidGuardianContact = randomGuardianContact;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidGuardianContactReferenceException =
                new InvalidGuardianContactReferenceException(foreignKeyConstraintConflictException);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(invalidGuardianContactReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianContactAsync(invalidGuardianContact))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                this.guardianContactService.AddGuardianContactAsync(invalidGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(invalidGuardianContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
