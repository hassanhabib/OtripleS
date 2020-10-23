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
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomContactId = default;
            Guid inputContactId = randomContactId;

            var invalidContactInputException = new InvalidContactException(
                parameterName: nameof(Contact.Id),
                parameterValue: inputContactId);

            var expectedContactValidationException = new ContactValidationException(invalidContactInputException);

            //when
            ValueTask<Contact> retrieveContactByIdTask =
                this.contactService.RetrieveContactByIdAsync(inputContactId);

            //then
            await Assert.ThrowsAsync<ContactValidationException>(() => retrieveContactByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                Times.Once
            );

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageContactIsNullAndLogItAsync()
        {
            //given
            Guid randomContactId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Contact invalidStorageContact = null;

            var notFoundContactException = new NotFoundContactException(inputContactId);

            var expectedContactValidationException = new ContactValidationException(notFoundContactException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectContactByIdAsync(inputContactId))
                .ReturnsAsync(invalidStorageContact);

            //when
            ValueTask<Contact> retrieveContactByIdTask =
                this.contactService.RetrieveContactByIdAsync(inputContactId);

            //then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                retrieveContactByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectContactByIdAsync(It.IsAny<Guid>()),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
