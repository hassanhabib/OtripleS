//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentContacts.Exceptions
{
    public class NotFoundStudentContactException : Exception
    {
        public NotFoundStudentContactException(Guid studentId, Guid contactId)
           : base($"Couldn't find StudentContact with StudentId: {studentId} " +
                  $"and ContactId: {contactId}.")
        { }
    }
}