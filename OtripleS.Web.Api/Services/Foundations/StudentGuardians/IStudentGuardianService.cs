// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Services.Foundations.StudentGuardians
{
    public interface IStudentGuardianService
    {
        ValueTask<StudentGuardian> AddStudentGuardianAsync(StudentGuardian studentGuardian);
        IQueryable<StudentGuardian> RetrieveAllStudentGuardians();
        ValueTask<StudentGuardian> RetrieveStudentGuardianByIdAsync(Guid studentId, Guid guardianId);
        ValueTask<StudentGuardian> ModifyStudentGuardianAsync(StudentGuardian studentGuardian);
        ValueTask<StudentGuardian> RemoveStudentGuardianByIdsAsync(Guid guardianId, Guid studentId);
    }
}
