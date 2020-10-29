// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;

namespace OtripleS.Web.Api.Services.UserContacts
{
	public partial class UserContactService
	{
		private void ValidateUserContactIdIsNull(Guid userId, Guid contactId)
		{
			if (userId == default)
			{
				throw new InvalidUserContactInputException(
					parameterName: nameof(UserContact.UserId),
					parameterValue: userId);
			}

			if (contactId == default)
			{
				throw new InvalidUserContactInputException(
					parameterName: nameof(UserContact.ContactId),
					parameterValue: contactId);
			}
		}
	}
}
