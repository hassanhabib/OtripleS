//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentContacts.Exceptions
{
    public class InvalidStudentContactInputException : Exception
    {
        public InvalidStudentContactInputException(string parameterName, object parameterValue)
            : base($"Invalid StudentContact, " +
                  $"ParameterName: {parameterName}, " +
                  $"ParameterValue: {parameterValue}.")
        { }
    }
}
