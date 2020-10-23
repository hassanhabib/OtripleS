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
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomContactId = default;
            Guid inputContactId = randomContactId;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.Id),
                parameterValue: inputContactId);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactInputException);

            // when
            ValueTask<Contact> deleteContactTask =
                this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() => deleteContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageContactIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Guid inputContactId = randomContact.Id;
            Contact inputContact = randomContact;
            Contact nullStorageContact = null;

            var notFoundContactException = new NotFoundContactException(inputContactId);

            var expectedContactValidationException =
                new ContactValidationException(notFoundContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(inputContactId))
                    .ReturnsAsync(nullStorageContact);

            // when
            ValueTask<Contact> actualContactTask =
                this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            await Assert.ThrowsAsync<ContactValidationException>(() => actualContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
