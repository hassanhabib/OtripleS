//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class LockedAssignmentAttachmentException : Exception
    {
        public LockedAssignmentAttachmentException(Exception innerException)
            : base(message: "Locked assignment attachment record exception, please try again later.",
                  innerException)
        { }
    }
}
