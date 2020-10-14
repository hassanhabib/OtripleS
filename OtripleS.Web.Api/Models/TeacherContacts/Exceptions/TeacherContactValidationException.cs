//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class TeacherContactValidationException : Exception
    {
        public TeacherContactValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException)
        { }
    }
}
