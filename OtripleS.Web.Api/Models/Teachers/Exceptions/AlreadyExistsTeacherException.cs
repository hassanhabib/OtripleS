// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class AlreadyExistsTeacherException : Xeption
    {
        public AlreadyExistsTeacherException(Exception innerException)
            : base(message: "Teacher with the same id already exists.", innerException) { }
    }
}
