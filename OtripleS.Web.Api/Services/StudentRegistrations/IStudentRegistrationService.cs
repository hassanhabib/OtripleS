using OtripleS.Web.Api.Models.StudentRegistrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public interface IStudentRegistrationService
    {
        ValueTask<StudentRegistration> AddStudentRegistrationAsync(StudentRegistration studentRegistration);
    }
}
