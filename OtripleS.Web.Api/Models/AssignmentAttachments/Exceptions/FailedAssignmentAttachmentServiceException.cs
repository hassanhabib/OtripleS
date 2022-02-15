// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class FailedAssignmentAttachmentServiceException : Xeption
    {
        public FailedAssignmentAttachmentServiceException(Exception innerException)
            : base(message: "Failed assignment attachemnt service error occurred, contact support.",
                  innerException)
        { }
    }
}
