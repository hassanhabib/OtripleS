//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUserContactIsNullAndLogItAsync()
        {
            // given
            UserContact randomUserContact = default;
            UserContact nullUserContact = randomUserContact;
            var nullUserContactException = new NullUserContactException();

            var expectedUserContactValidationException =
                new UserContactValidationException(nullUserContactException);

            // when
            ValueTask<UserContact> addUserContactTask =
                this.userContactService.AddUserContactAsync(nullUserContact);

            // then
            await Assert.ThrowsAsync<UserContactValidationException>(() =>
                addUserContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserContactAsync(It.IsAny<UserContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
