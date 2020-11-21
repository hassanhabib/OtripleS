// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentExams;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<StudentExam> InsertStudentExamAsync(StudentExam studentExam);
        public IQueryable<StudentExam> SelectAllStudentExams();
        public ValueTask<StudentExam> SelectStudentExamByIdAsync(Guid studentExamId);
        public ValueTask<StudentExam> UpdateStudentExamAsync(StudentExam studentExam);
        public ValueTask<StudentExam> DeleteStudentExamAsync(StudentExam studentExam);
    }
}
