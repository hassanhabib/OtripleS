using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public partial class StudentRegistrationService
    {
        private static void ValidateStudentRegistrationOnCreate(StudentRegistration studentRegistration)
        {
            ValidateStudentRegistrationIsNull(studentRegistration);
        }

        private static void ValidateStudentRegistrationIsNull(StudentRegistration studentRegistration)
        {
            if (studentRegistration is null)
            {
                throw new NullStudentRegistrationException();
            }
        }
    }
}
