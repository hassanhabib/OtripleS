// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;

namespace OtripleS.Web.Api.Services.UserContacts
{
	public partial class UserContactService
	{
		private delegate ValueTask<UserContact> ReturningUserContactFunction();

		private async ValueTask<UserContact> TryCatch(ReturningUserContactFunction returningUserContactFunction)
		{
			try
			{
				return await returningUserContactFunction();
			}
			catch (InvalidUserContactInputException invalidUserContactInputException)
			{
				throw CreateAndLogValidationException(invalidUserContactInputException);
			}
			catch (NotFoundUserContactException notFoundUserContactException)
			{
				throw CreateAndLogValidationException(notFoundUserContactException);
			}
			catch (SqlException sqlException)
			{
				throw CreateAndLogCriticalDependencyException(sqlException);
			}
		}

		private UserContactValidationException CreateAndLogValidationException(Exception exception)
		{
			var UserContactValidationException = new UserContactValidationException(exception);
			this.loggingBroker.LogError(UserContactValidationException);

			return UserContactValidationException;
		}

		private UserContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
		{
			var UserContactDependencyException = new UserContactDependencyException(exception);
			this.loggingBroker.LogCritical(UserContactDependencyException);

			return UserContactDependencyException;
		}

		private UserContactDependencyException CreateAndLogDependencyException(Exception exception)
		{
			var UserContactDependencyException = new UserContactDependencyException(exception);
			this.loggingBroker.LogError(UserContactDependencyException);

			return UserContactDependencyException;
		}

		private UserContactServiceException CreateAndLogServiceException(Exception exception)
		{
			var UserContactServiceException = new UserContactServiceException(exception);
			this.loggingBroker.LogError(UserContactServiceException);

			return UserContactServiceException;
		}
	}
}
