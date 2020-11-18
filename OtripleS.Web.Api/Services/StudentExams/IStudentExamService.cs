//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentExams;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.StudentExams
{
    public interface IStudentExamService
    {
        ValueTask<StudentExam> AddStudentExamAsync(StudentExam studentExam);
    }
}
