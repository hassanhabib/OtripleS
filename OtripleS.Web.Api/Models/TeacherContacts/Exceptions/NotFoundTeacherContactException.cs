// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class NotFoundTeacherContactException : Exception
    {
        public NotFoundTeacherContactException(Guid teacherId, Guid contactId)
           : base($"Couldn't find TeacherContact with TeacherId: {teacherId} " +
                  $"and ContactId: {contactId}.")
        { }
    }
}
