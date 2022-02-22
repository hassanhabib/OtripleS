// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class NotFoundTeacherException : Xeption
    {
        public NotFoundTeacherException(Guid teacherId)
            : base(message: $"Couldn't find teacher with id: {teacherId}.") { }
    }
}
