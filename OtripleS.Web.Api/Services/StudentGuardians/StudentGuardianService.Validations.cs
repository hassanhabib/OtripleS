// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;

namespace OtripleS.Web.Api.Services.StudentGuardians
{
	public partial class StudentGuardianService
	{
        private void ValidateStudentGuardianOnModify(StudentGuardian studentGuardian)
        {
            ValidateStudentGuardianIsNull(studentGuardian);
            ValidateStudentGuardianIdIsNull(studentGuardian.StudentId, studentGuardian.GuardianId);
        }

        private void ValidateStudentGuardianIsNull(StudentGuardian studentGuardian)
        {
            if (studentGuardian is null)
            {
                throw new NullStudentGuardianException();
            }
        }

        private void ValidateStudentGuardianIdIsNull(Guid studentId, Guid guardianId)
        {
            if (studentId == default)
            {
                throw new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.StudentId),
                    parameterValue: studentId);
            }
        }
    }
}
