﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class AlreadyExistsCourseException : Exception
    {
        public AlreadyExistsCourseException(Exception innerException)
            : base(message: "Course with the same id already exists.", innerException) { }
    }
}
