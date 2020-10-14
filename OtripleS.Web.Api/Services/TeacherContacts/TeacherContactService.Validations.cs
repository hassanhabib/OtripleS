//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;

namespace OtripleS.Web.Api.Services.TeacherContacts
{
    public partial class TeacherContactService
	{
		private void ValidateTeacherContactIdIsNull(Guid teacherId, Guid contactId)
		{
			if (teacherId == default)
			{
				throw new InvalidTeacherContactInputException(
					parameterName: nameof(TeacherContact.TeacherId),
					parameterValue: teacherId);
			}

			if (contactId == default)
			{
				throw new InvalidTeacherContactInputException(
					parameterName: nameof(TeacherContact.ContactId),
					parameterValue: contactId);
			}
		}
	}
}