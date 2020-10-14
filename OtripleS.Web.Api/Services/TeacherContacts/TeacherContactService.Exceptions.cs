//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;

namespace OtripleS.Web.Api.Services.TeacherContacts
{
    public partial class TeacherContactService
	{
        private delegate ValueTask<TeacherContact> ReturningTeacherContactFunction();

		private async ValueTask<TeacherContact> TryCatch(
			ReturningTeacherContactFunction returningTeacherContactFunction)
		{
			try
			{
				return await returningTeacherContactFunction();
			}
			catch (InvalidTeacherContactInputException invalidTeacherContactInputException)
			{
				throw CreateAndLogValidationException(invalidTeacherContactInputException);
			}
			catch (NotFoundTeacherContactException notFoundTeacherContactException)
			{
				throw CreateAndLogValidationException(notFoundTeacherContactException);
			}
		}


		private TeacherContactValidationException CreateAndLogValidationException(Exception exception)
		{
			var TeacherContactValidationException = new TeacherContactValidationException(exception);
			this.loggingBroker.LogError(TeacherContactValidationException);

			return TeacherContactValidationException;
		}
	}
}