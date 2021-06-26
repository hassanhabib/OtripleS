// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class InvalidTeacherContactReferenceException : Exception
    {
        public InvalidTeacherContactReferenceException(Exception innerException)
            : base("Invalid teacher contact reference error occurred.", innerException) { }
    }
}
