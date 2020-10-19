//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentContacts.Exceptions
{
    public class LockedStudentContactException : Exception
    {
        public LockedStudentContactException(Exception innerException)
            : base("Locked StudentContact record exception, please try again later.", innerException) { }
    }
}