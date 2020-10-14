//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
			catch (SqlException sqlException)
			{
				throw CreateAndLogCriticalDependencyException(sqlException);
			}
		}


		private TeacherContactValidationException CreateAndLogValidationException(Exception exception)
		{
			var teacherContactValidationException = new TeacherContactValidationException(exception);
			this.loggingBroker.LogError(teacherContactValidationException);

			return teacherContactValidationException;
		}
		private teacherContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
		{
			var teacherContactDependencyException = new teacherContactDependencyException(exception);
			this.loggingBroker.LogCritical(teacherContactDependencyException);

			return teacherContactDependencyException;
		}

		private teacherContactDependencyException CreateAndLogDependencyException(Exception exception)
		{
			var TeacherContactDependencyException = new teacherContactDependencyException(exception);
			this.loggingBroker.LogError(TeacherContactDependencyException);

			return TeacherContactDependencyException;
		}
	}
}