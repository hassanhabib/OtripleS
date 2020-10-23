// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenContactIsNullAndLogItAsync()
        {
            //given
            Contact invalidContact = null;
            var nullContactException = new NullContactException();

            var expectedContactValidationException =
                new ContactValidationException(nullContactException);

            //when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(invalidContact);

            //then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenContactIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidContactId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact invalidContact = randomContact;
            invalidContact.Id = invalidContactId;

            var invalidContactException = new InvalidContactException(
                parameterName: nameof(Contact.Id),
                parameterValue: invalidContact.Id);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

            //when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(invalidContact);

            //then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            inputContact.CreatedBy = default;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.CreatedBy),
                parameterValue: inputContact.CreatedBy);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactInputException);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(inputContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            inputContact.UpdatedBy = default;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.UpdatedBy),
                parameterValue: inputContact.UpdatedBy);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactInputException);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(inputContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            inputContact.CreatedDate = default;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.CreatedDate),
                parameterValue: inputContact.CreatedDate);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactInputException);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(inputContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            inputContact.UpdatedDate = default;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.UpdatedDate),
                parameterValue: inputContact.UpdatedDate);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactInputException);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(inputContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.UpdatedDate),
                parameterValue: inputContact.UpdatedDate);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactInputException);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(inputContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            inputContact.UpdatedBy = inputContact.CreatedBy;
            inputContact.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.UpdatedDate),
                parameterValue: inputContact.UpdatedDate);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(inputContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfContactDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact nonExistentContact = randomContact;
            nonExistentContact.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Contact noContact = null;
            var notFoundContactException = new NotFoundContactException(nonExistentContact.Id);

            var expectedContactValidationException =
                new ContactValidationException(notFoundContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(nonExistentContact.Id))
                    .ReturnsAsync(noContact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(nonExistentContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(nonExistentContact.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(randomDate);
            Contact invalidContact = randomContact;
            invalidContact.UpdatedDate = randomDate;
            Contact storageContact = randomContact.DeepClone();
            Guid contactId = invalidContact.Id;
            invalidContact.CreatedDate = storageContact.CreatedDate.AddMinutes(randomNumber);

            var invalidContactException = new InvalidContactException(
                parameterName: nameof(Contact.CreatedDate),
                parameterValue: invalidContact.CreatedDate);

            var expectedContactValidationException =
              new ContactValidationException(invalidContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(contactId))
                    .ReturnsAsync(storageContact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(invalidContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(invalidContact.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(randomDate);
            Contact invalidContact = randomContact;
            invalidContact.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Contact storageContact = randomContact.DeepClone();
            Guid contactId = invalidContact.Id;
            invalidContact.CreatedBy = invalidCreatedBy;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.CreatedBy),
                parameterValue: invalidContact.CreatedBy);

            var expectedContactValidationException =
              new ContactValidationException(invalidContactInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(contactId))
                    .ReturnsAsync(storageContact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(invalidContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(invalidContact.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(randomDate);
            randomContact.CreatedDate = randomContact.CreatedDate.AddMinutes(minutesInThePast);
            Contact invalidContact = randomContact;
            invalidContact.UpdatedDate = randomDate;
            Contact storageContact = randomContact.DeepClone();
            Guid contactId = invalidContact.Id;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.UpdatedDate),
                parameterValue: invalidContact.UpdatedDate);

            var expectedContactValidationException =
              new ContactValidationException(invalidContactInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(contactId))
                    .ReturnsAsync(storageContact);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(invalidContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(invalidContact.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
