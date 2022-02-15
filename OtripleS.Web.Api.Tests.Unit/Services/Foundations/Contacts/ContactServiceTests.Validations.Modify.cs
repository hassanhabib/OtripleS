// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Contacts
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedContactValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnModifyIfContactIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidContact = new Contact
            {
                Information = invalidText,
                Notes = invalidText
            };

            var invalidContactException = new InvalidContactException();

            invalidContactException.AddData(
                key: nameof(Contact.Id),
                values: "Id is required");

            invalidContactException.AddData(
                key: nameof(Contact.Information),
                values: "Text is required");

            invalidContactException.AddData(
                key: nameof(Contact.Notes),
                values: "Text is required");

            invalidContactException.AddData(
                key: nameof(Contact.CreatedDate),
                values: "Date is required");

            invalidContactException.AddData(
                key: nameof(Contact.UpdatedDate),
                values: new string[] {
                    "Date is required",
                    $"Date is the same as {nameof(Contact.CreatedDate)}"
                });

            invalidContactException.AddData(
                key: nameof(Contact.CreatedBy),
                values: "Id is required");

            invalidContactException.AddData(
                key: nameof(Contact.UpdatedBy),
                values: "Id is required");

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

            // when
            ValueTask<Contact> createContactTask =
                this.contactService.ModifyContactAsync(invalidContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                createContactTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedContactValidationException))),
                        Times.Once);

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
            var invalidContactException = new InvalidContactException();

            invalidContactException.AddData(
                key: nameof(Contact.UpdatedDate),
                values: $"Date is not recent");

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

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
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedContactValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedContactValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedContactValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedContactValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedContactValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
