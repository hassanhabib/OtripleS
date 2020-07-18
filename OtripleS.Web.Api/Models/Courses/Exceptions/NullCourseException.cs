// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------


using System;

namespace OtripleS.Web.Api.Models.Courses.Exceptions
{
    public class NullCourseException : Exception
    {
        public NullCourseException() : base("The course is null.")
        {
        }
    }
}
