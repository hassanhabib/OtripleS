// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Exams;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
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
