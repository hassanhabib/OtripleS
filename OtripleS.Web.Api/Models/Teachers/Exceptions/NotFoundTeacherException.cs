// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class NotFoundTeacherException : Exception
    {
        public NotFoundTeacherException(Guid teacherId)
            : base($"Couldn't find teacher with Id: {teacherId}.") { }
    }
}
