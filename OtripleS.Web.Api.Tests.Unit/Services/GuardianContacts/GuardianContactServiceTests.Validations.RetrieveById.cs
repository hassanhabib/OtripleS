//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid randomGuardianId = default;
            Guid inputContactId = randomContactId;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianContactInputException = new InvalidGuardianContactInputException(
                parameterName: nameof(GuardianContact.GuardianId),
                parameterValue: inputGuardianId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(invalidGuardianContactInputException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(inputGuardianId, inputContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() => retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenContactIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomContactId = default;
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputGuardianId = randomGuardianId;

            var invalidGuardianContactInputException = new InvalidGuardianContactInputException(
                parameterName: nameof(GuardianContact.ContactId),
                parameterValue: inputContactId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(invalidGuardianContactInputException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(inputGuardianId, inputContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() => retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageGuardianContactIsInvalidAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            Guid inputContactId = randomGuardianContact.ContactId;
            Guid inputGuardianId = randomGuardianContact.GuardianId;
            GuardianContact nullStorageGuardianContact = null;

            var notFoundGuardianContactException =
                new NotFoundGuardianContactException(inputGuardianId, inputContactId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(notFoundGuardianContactException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianContactByIdAsync(inputGuardianId, inputContactId))
                    .ReturnsAsync(nullStorageGuardianContact);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(inputGuardianId, inputContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() =>
                retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
