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
           : base(message: $"Couldn't find student contact with student id: {studentId} " +
                  $"and contact id: {contactId}.")
        { }
    }
}