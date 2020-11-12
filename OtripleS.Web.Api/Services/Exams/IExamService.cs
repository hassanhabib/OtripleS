// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Services.Exams
{
	public interface IExamService
	{
		ValueTask<Exam> AddExamAsync(Exam exam);
		ValueTask<Exam> RetrieveExamByIdAsync(Guid examId);
		ValueTask<Exam> DeleteExamByIdAsync(Guid examId);
	}
}
