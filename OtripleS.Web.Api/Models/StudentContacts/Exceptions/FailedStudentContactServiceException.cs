// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.StudentContacts.Exceptions
{
    public class FailedStudentContactServiceException : Xeption
    {
        public FailedStudentContactServiceException(Exception innerException)
            : base(message: "Failed student contact service error occured." ,innerException)
        {
        }
    }
}
