// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class AlreadyExistsTeacherContactException : Exception
    {
        public AlreadyExistsTeacherContactException(Exception innerException)
            : base("Teacher Contact with the same id already exists.", innerException) { }
    }
}
