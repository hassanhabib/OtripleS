// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class FailedTeacherContactServiceException : Xeption
    {
        public FailedTeacherContactServiceException(Exception innerException)
            : base(message: "Failed teacher contact service error occured.", innerException)
        {
        }
    }
}
