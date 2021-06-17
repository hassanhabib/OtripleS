// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Foundations.StudentExams.Exceptions
{
    public class InvalidStudentExamInputException : Exception
    {
        public InvalidStudentExamInputException(string parameterName, object parameterValue)
            : base($"Invalid Student exam, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
