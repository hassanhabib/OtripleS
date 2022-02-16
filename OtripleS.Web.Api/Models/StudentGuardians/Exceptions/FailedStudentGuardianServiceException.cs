// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.StudentGuardians.Exceptions
{
    public class FailedStudentGuardianServiceException : Xeption
    {
       public FailedStudentGuardianServiceException(Exception innerException)
            : base(message: "Failed student guardian sevice error occured.",innerException)
        {
        }
    }
}
