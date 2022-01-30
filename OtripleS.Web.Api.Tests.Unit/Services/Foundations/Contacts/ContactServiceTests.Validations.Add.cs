// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenContactIsNullAndLogItAsync()
        {
            // given
            Contact invalidContact = null;
            

            var nullContactException = new NullContactException();

            var expectedContactValidationException =
                new ContactValidationException(nullContactException);

            // when
            ValueTask<Contact> addContactTask =
                this.contactService.AddContactAsync(invalidContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                addContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnCreateIfContactIsInvalidAndLogItAsync(
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
                values: "Date is required");

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
                this.contactService.AddContactAsync(invalidContact);

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

            this.storageBrokerMock.Verify(broker =>
                broker.InsertContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact invalidContact = randomContact;
            invalidContact.UpdatedBy = Guid.NewGuid();
            var invalidContactException = new InvalidContactException();

            invalidContactException.AddData(
                key: nameof(Contact.UpdatedBy),
                values: $"Id is not the same as {nameof(Contact.CreatedBy)}");

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Contact> createContactTask =
                this.contactService.AddContactAsync(invalidContact);

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

            this.storageBrokerMock.Verify(broker =>
                broker.InsertContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact invalidContact = randomContact;
            invalidContact.UpdatedDate = GetRandomDateTime();
            var invalidContactException = new InvalidContactException();

            invalidContactException.AddData(
                key: nameof(Contact.UpdatedDate),
                values: $"Date is not the same as {nameof(Contact.CreatedDate)}");

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Contact> createContactTask =
                this.contactService.AddContactAsync(invalidContact);

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

            this.storageBrokerMock.Verify(broker =>
                broker.InsertContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(randomDate);
            Contact invalidContact = randomContact;
            invalidContact.CreatedDate = randomDate.AddMinutes(minutes);
            invalidContact.UpdatedDate = invalidContact.CreatedDate;
            var invalidContactException = new InvalidContactException();

            invalidContactException.AddData(
                key: nameof(Contact.CreatedDate),
                values: $"Date is not recent");

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Contact> createContactTask =
                this.contactService.AddContactAsync(invalidContact);

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

            this.storageBrokerMock.Verify(broker =>
                broker.InsertContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenContactAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact alreadyExistsContact = randomContact;
            alreadyExistsContact.UpdatedBy = alreadyExistsContact.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsContactException =
                new AlreadyExistsContactException(duplicateKeyException);

            var expectedContactValidationException =
                new ContactValidationException(alreadyExistsContactException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertContactAsync(alreadyExistsContact))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Contact> addContactTask =
                this.contactService.AddContactAsync(alreadyExistsContact);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                addContactTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertContactAsync(alreadyExistsContact),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
