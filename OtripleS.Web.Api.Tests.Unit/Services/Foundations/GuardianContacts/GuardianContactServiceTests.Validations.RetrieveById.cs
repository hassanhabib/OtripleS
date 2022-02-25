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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenGuardianIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidContactId = Guid.NewGuid();
            Guid invalidGuardianId = Guid.Empty;

            var invalidGuardianContactException = new InvalidGuardianContactException(
                parameterName: nameof(GuardianContact.ContactId),
                parameterValue: invalidContactId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(invalidGuardianContactException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(invalidGuardianId, invalidContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() => retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianContactValidationException))),
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
            Guid invalidContactId = Guid.Empty;
            Guid invalidGuardianId = Guid.NewGuid();

            var invalidGuardianContactException = new InvalidGuardianContactException(
                parameterName: nameof(GuardianContact.ContactId),
                parameterValue: invalidContactId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(invalidGuardianContactException);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(invalidGuardianId, invalidContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() => retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianContactValidationException))),
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
            Guid invalidContactId = randomGuardianContact.ContactId;
            Guid invalidGuardianId = randomGuardianContact.GuardianId;
            GuardianContact nullStorageGuardianContact = null;

            var notFoundGuardianContactException =
                new NotFoundGuardianContactException(invalidGuardianId, invalidContactId);

            var expectedGuardianContactValidationException =
                new GuardianContactValidationException(notFoundGuardianContactException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianContactByIdAsync(invalidGuardianId, invalidContactId))
                    .ReturnsAsync(nullStorageGuardianContact);

            // when
            ValueTask<GuardianContact> retrieveGuardianContactTask =
                this.guardianContactService.RetrieveGuardianContactByIdAsync(invalidGuardianId, invalidContactId);

            // then
            await Assert.ThrowsAsync<GuardianContactValidationException>(() =>
                retrieveGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianContactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
