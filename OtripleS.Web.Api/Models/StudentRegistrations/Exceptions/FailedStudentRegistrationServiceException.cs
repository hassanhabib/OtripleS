// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.StudentRegistrations.Exceptions
{
    public class FailedStudentRegistrationServiceException : Xeption
    {
        public FailedStudentRegistrationServiceException(Exception innerException)
            : base(message: "Failed student registration service error occured.", innerException)
        {
        }
    }
}
