// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class InvalidAssignmentAttachmentException : Exception
    {
        public InvalidAssignmentAttachmentException(string parameterName, object parameterValue)
            : base(message: $"Invalid assignment attachment, " +
                 $"parameter name: {parameterName}, " +
                 $"parameter value: {parameterValue}.")
        { }
    }
}
