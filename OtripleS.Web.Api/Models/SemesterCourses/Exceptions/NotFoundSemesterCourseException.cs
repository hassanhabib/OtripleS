// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.SemesterCourses.Exceptions
{
    public class NotFoundSemesterCourseException : Exception
    {
        public NotFoundSemesterCourseException(Guid semesterCourseId)
            : base(message: $"Couldn't find semester course with id: {semesterCourseId}.") { }
    }
}
