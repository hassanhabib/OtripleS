//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.Students;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services
{
    public interface IStudentService
    {
        ValueTask<Student> RegisterStudentAsync(Student student);
        ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId);
        ValueTask<Student> ModifyStudentAsync(Student student);
        ValueTask<Student> DeleteStudentAsync(Guid studentId);
        IQueryable<Student> RetrieveAllStudents();
    }
}
