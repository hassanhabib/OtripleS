// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using OtripleS.Web.Api.Services.Contacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenContactIsNullAndLogItAsync()
        {
            // given
            Contact randomContact = null;
            Contact nullContact = randomContact;

            var nullContactException = new NullContactException();

            var expectedContactValidationException =
                new ContactValidationException(nullContactException);

            // when
            ValueTask<Contact> addContactTask =
                this.contactService.AddContactAsync(nullContact);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            inputContact.Id = Guid.Empty;

            var invalidContactException = new InvalidContactException(
                parameterName: nameof(Contact.Id),
                parameterValue: inputContact.Id);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

            // when
            ValueTask<Contact> addContactTask =
                this.contactService.AddContactAsync(inputContact);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<Contact> addContactTask =
                this.contactService.AddContactAsync(inputContact);

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
    }
}
