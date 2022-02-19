// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Services.Foundations.StudentRegistrations
{
    public interface IStudentRegistrationService
    {
        IQueryable<StudentRegistration> RetrieveAllStudentRegistrations();
        ValueTask<StudentRegistration> AddStudentRegistrationAsync(StudentRegistration studentRegistration);
        ValueTask<StudentRegistration> RetrieveStudentRegistrationByIdAsync(Guid studentId, Guid registrationId);
        ValueTask<StudentRegistration> RemoveStudentRegistrationByIdsAsync(Guid studentId, Guid registrationId);
    }
}
