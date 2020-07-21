// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class InvalidClassroomException : Exception
    {
        public InvalidClassroomException(string parameterName, object parameterValue)
            : base($"Invalid Classroom, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
