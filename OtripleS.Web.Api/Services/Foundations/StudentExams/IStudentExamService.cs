// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Foundations.StudentExams;

namespace OtripleS.Web.Api.Services.Foundations.StudentExams
{
    public interface IStudentExamService
    {
        ValueTask<StudentExam> AddStudentExamAsync(StudentExam studentExam);

        ValueTask<StudentExam> RetrieveStudentExamByIdAsync(Guid studentExamId);

        IQueryable<StudentExam> RetrieveAllStudentExams();

        ValueTask<StudentExam> RemoveStudentExamByIdAsync(Guid studentExamId);

        ValueTask<StudentExam> ModifyStudentExamAsync(StudentExam studentExam);
    }
}
