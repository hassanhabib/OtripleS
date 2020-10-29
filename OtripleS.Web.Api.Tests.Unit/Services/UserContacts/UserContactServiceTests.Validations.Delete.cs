// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
		public async Task ShouldThrowValidatonExceptionOnRemoveWhenUserIdIsInvalidAndLogItAsync()
		{
			// given
			Guid randomContactId = Guid.NewGuid();
			Guid randomUserId = default;
			Guid inputContactId = randomContactId;
			Guid inputUserId = randomUserId;

			var invalidUserContactInputException = new InvalidUserContactInputException(
				parameterName: nameof(UserContact.UserId),
				parameterValue: inputUserId);

			var expectedUserContactValidationException =
				new UserContactValidationException(invalidUserContactInputException);

			// when
			ValueTask<UserContact> removeUserContactTask =
				this.userContactService.RemoveUserContactByIdAsync(inputUserId, inputContactId);

			// then
			await Assert.ThrowsAsync<UserContactValidationException>(() => removeUserContactTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedUserContactValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectUserContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
					Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidatonExceptionOnRemoveWhenContactIdIsInvalidAndLogItAsync()
		{
			// given
			Guid randomContactId = default;
			Guid randomUserId = Guid.NewGuid();
			Guid inputContactId = randomContactId;
			Guid inputUserId = randomUserId;

			var invalidUserContactInputException = new InvalidUserContactInputException(
				parameterName: nameof(UserContact.ContactId),
				parameterValue: inputContactId);

			var expectedUserContactValidationException =
				new UserContactValidationException(invalidUserContactInputException);

			// when
			ValueTask<UserContact> removeUserContactTask =
				this.userContactService.RemoveUserContactByIdAsync(inputUserId, inputContactId);

			// then
			await Assert.ThrowsAsync<UserContactValidationException>(() => removeUserContactTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedUserContactValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectUserContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteUserContactAsync(It.IsAny<UserContact>()),
					Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
