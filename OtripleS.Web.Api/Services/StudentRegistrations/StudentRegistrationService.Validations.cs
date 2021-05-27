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
            ValidateStudentRegistrationIds(studentRegistration.StudentId, studentRegistration.RegistrationId);
        }

        private static void ValidateStudentRegistrationIsNull(StudentRegistration studentRegistration)
        {
            if (studentRegistration is null)
            {
                throw new NullStudentRegistrationException();
            }
        }

        private static void ValidateStudentRegistrationIds(Guid studentId, Guid registrationId)
        {
            switch (studentId, registrationId)
            {
                case { } when studentId == default:
                    throw new InvalidStudentRegistrationException(
                        parameterName: nameof(StudentRegistration.StudentId),
                        parameterValue: studentId);

                case { } when registrationId == default:
                    throw new InvalidStudentRegistrationException(
                        parameterName: nameof(StudentRegistration.RegistrationId),
                        parameterValue: registrationId);
            }
        }
    }
}
