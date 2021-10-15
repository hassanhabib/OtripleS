// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentExams;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentExam> InsertStudentExamAsync(StudentExam studentExam);
        IQueryable<StudentExam> SelectAllStudentExams();
        ValueTask<StudentExam> SelectStudentExamByIdAsync(Guid studentExamId);
        ValueTask<StudentExam> UpdateStudentExamAsync(StudentExam studentExam);
        ValueTask<StudentExam> DeleteStudentExamAsync(StudentExam studentExam);
    }
}