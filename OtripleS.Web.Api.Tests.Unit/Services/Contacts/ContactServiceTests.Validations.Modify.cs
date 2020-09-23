// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
    }
}
