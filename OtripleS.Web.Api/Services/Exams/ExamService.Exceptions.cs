// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
		}

		private ExamValidationException CreateAndLogValidationException(Exception exception)
		{
			var courseValidationException = new ExamValidationException(exception);
			this.loggingBroker.LogError(courseValidationException);

			return courseValidationException;
		}
	}
}
