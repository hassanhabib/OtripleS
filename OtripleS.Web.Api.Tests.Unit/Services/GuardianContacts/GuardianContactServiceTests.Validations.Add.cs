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
    }
}
