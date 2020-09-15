// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;

namespace OtripleS.Web.Api.Services.StudentGuardians
{
	public partial class StudentGuardianService
	{
        private void ValidateStudentGuardianOnModify(StudentGuardian studentGuardian)
        {
            ValidateStudentGuardianIsNull(studentGuardian);
        }

        private void ValidateStudentGuardianIsNull(StudentGuardian studentGuardian)
        {
            if (studentGuardian is null)
            {
                throw new NullStudentGuardianException();
            }
        }
    }
}
