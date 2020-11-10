// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;

namespace OtripleS.Web.Api.Services.Exams
{
	public partial class ExamService
	{
		private delegate ValueTask<Exam> ReturningExamFunction();

		private async ValueTask<Exam> TryCatch(ReturningExamFunction returningExamFunction)
		{
			try
			{
				return await returningExamFunction();
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
			catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
			{
				var lockedCourseException = new LockedExamException(dbUpdateConcurrencyException);

				throw CreateAndLogDependencyException(lockedCourseException);
			}
			catch (DbUpdateException dbUpdateException)
			{
				throw CreateAndLogDependencyException(dbUpdateException);
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
	}
}
