// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Exams.Exceptions
{
    public class NotFoundExamException : Exception
	{
		public NotFoundExamException(Guid examId)
			: base($"Couldn't find exam with Id: {examId}.") { }
	}
}
