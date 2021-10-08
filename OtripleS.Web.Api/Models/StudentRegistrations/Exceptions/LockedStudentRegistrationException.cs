//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class LockedStudentRegistrationException : Exception
    {
        public LockedStudentRegistrationException(Exception innerException)
            : base(message: "Locked student registration record exception, please try again later.", innerException) { }
    }
}
