// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class InvalidStudentExamFeeException : Exception
    {
        public InvalidStudentExamFeeException(string parameterName, object parameterValue)
            : base($"Invalid StudentExamFee, " + 
                  $"ParameterNmae: {parameterName}, " + 
                  $"ParameterValue: {parameterValue}.")
        {

        }
    }
}
