// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.SemesterCourses.Exceptions
{
    public class InvalidSemesterCourseException : Exception
    {
        public InvalidSemesterCourseException(string parameterName, object parameterValue)
            : base(message: $"Invalid semester course, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
