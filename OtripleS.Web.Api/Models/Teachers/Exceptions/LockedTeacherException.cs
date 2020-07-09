// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class LockedTeacherException : Exception
    {
        public LockedTeacherException(Exception innerException)
            : base("Locked teacher record exception, please try again later.", innerException) { }
    }
}
