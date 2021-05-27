// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public interface IStudentRegistrationService
    {
        IQueryable<StudentRegistration> RetrieveAllStudentRegistrations();
    }
}
