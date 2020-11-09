// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Exam> InsertExamAsync(Exam exam);
        IQueryable<Exam> SelectAllExams();
        ValueTask<Exam> SelectExamByIdAsync(Guid examId);
        ValueTask<Exam> UpdateExamAsync(Exam exam);
        ValueTask<Exam> DeleteExamAsync(Exam exam);
    }
}
