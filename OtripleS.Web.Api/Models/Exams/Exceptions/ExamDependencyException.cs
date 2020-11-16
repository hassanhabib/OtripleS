// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class ExamDependencyException : Exception
	{
		public ExamDependencyException(Exception innerException)
			: base("Service dependency error occurred, contact support.", innerException) { }
	}
}
