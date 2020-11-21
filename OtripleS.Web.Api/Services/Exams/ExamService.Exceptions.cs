// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;

namespace OtripleS.Web.Api.Services.Exams
{
    public partial class ExamService
	{
		private delegate ValueTask<Exam> ReturningExamFunction();
		private delegate IQueryable<Exam> ReturningQueryableExamFunction();
		
		private async ValueTask<Exam> TryCatch(ReturningExamFunction returningExamFunction)
		{
			try
			{
				return await returningExamFunction();
			}
			catch (NullExamException nullExamException)
			{
				throw CreateAndLogValidationException(nullExamException);
			}
			catch (InvalidExamInputException invalidExamInputException)
			{
				throw CreateAndLogValidationException(invalidExamInputException);
			}
			catch (NotFoundExamException nullExamException)
			{
				throw CreateAndLogValidationException(nullExamException);
			}
			catch (SqlException sqlException)
			{
				throw CreateAndLogCriticalDependencyException(sqlException);
			}
			catch (DuplicateKeyException duplicateKeyException)
			{
				var alreadyExistsExamException =
					new AlreadyExistsExamException(duplicateKeyException);

				throw CreateAndLogValidationException(alreadyExistsExamException);
			}
			catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
			{
				var lockedExamException = new LockedExamException(dbUpdateConcurrencyException);

				throw CreateAndLogDependencyException(lockedExamException);
			}
			catch (DbUpdateException dbUpdateException)
			{
				throw CreateAndLogDependencyException(dbUpdateException);
			}
			catch (Exception exception)
			{
				throw CreateAndLogServiceException(exception);
			}
		}

		private IQueryable<Exam> TryCatch(ReturningQueryableExamFunction returningQueryableExamFunction)
        {
			try
			{
				return returningQueryableExamFunction();
			}
			catch (SqlException sqlException)
			{
				throw CreateAndLogCriticalDependencyException(sqlException);
			}
			catch (DbUpdateException dbUpdateException)
			{
				throw CreateAndLogDependencyException(dbUpdateException);
			}
			catch (Exception exception)
			{
				throw CreateAndLogServiceException(exception);
			}
		}

		private ExamValidationException CreateAndLogValidationException(Exception exception)
		{
			var examValidationException = new ExamValidationException(exception);
			this.loggingBroker.LogError(examValidationException);

			return examValidationException;
		}

		private ExamDependencyException CreateAndLogCriticalDependencyException(Exception exception)
		{
			var examDependencyException = new ExamDependencyException(exception);
			this.loggingBroker.LogCritical(examDependencyException);

			return examDependencyException;
		}

		private ExamDependencyException CreateAndLogDependencyException(Exception exception)
		{
			var examDependencyException = new ExamDependencyException(exception);
			this.loggingBroker.LogError(examDependencyException);

			return examDependencyException;
		}

		private ExamServiceException CreateAndLogServiceException(Exception exception)
		{
			var examServiceException = new ExamServiceException(exception);
			this.loggingBroker.LogError(examServiceException);

			return examServiceException;
		}
	}
}
