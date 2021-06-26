// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class LockedTeacherContactException : Exception
    {
        public LockedTeacherContactException(Exception innerException)
            : base("Locked TeacherContact record exception, please try again later.", innerException) { }
    }
}
