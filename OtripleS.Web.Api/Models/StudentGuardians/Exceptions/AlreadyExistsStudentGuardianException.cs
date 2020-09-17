// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentGuardians.Exceptions
{
    public class AlreadyExistsStudentGuardianException : Exception
    {
        public AlreadyExistsStudentGuardianException(Exception innerException)
            : base("Student Guardian with the same id already exists.", innerException) { }
    }
}
