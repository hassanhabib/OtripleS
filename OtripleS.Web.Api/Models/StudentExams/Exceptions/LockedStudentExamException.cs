// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExams.Exceptions
{
	public class LockedStudentExamException : Exception
	{
		public LockedStudentExamException(Exception innerException)
			: base("Locked studentExam record exception, please try again later.", innerException) { }
	}
}
