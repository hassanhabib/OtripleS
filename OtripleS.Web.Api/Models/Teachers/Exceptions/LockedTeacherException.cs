// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class LockedTeacherException : Xeption
    {
        public LockedTeacherException(Exception innerException)
            : base(message: "Locked teacher error occurred, please try again later.", innerException) { }
    }
}
