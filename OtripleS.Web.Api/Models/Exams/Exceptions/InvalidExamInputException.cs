// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class InvalidExamInputException : Exception
	{
		public InvalidExamInputException(string parameterName, object parameterValue)
			: base($"Invalid Exam, " +
				  $"ParameterName: {parameterName}, " +
				  $"ParameterValue: {parameterValue}.")
		{ }
	}
}