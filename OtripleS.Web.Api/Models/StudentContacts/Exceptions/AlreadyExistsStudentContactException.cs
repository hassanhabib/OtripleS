// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentContacts.Exceptions
{
    public class AlreadyExistsStudentContactException : Exception
    {
        public AlreadyExistsStudentContactException(Exception innerException)
            : base("Student Contact with the same id already exists.", innerException) { }
    }
}
