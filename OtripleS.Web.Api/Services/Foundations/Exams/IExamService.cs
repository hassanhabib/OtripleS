// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Exams;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.Exams
{
    public interface IExamService
    {
        ValueTask<Exam> AddExamAsync(Exam exam);
        IQueryable<Exam> RetrieveAllExams();
        ValueTask<Exam> RetrieveExamByIdAsync(Guid examId);
        ValueTask<Exam> ModifyExamAsync(Exam exam);
        ValueTask<Exam> RemoveExamByIdAsync(Guid examId);
    }
}
