// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.TeacherAttachments.Exceptions
{
    public class FailedTeacherAttachmentServiceException : Xeption
    {
        public FailedTeacherAttachmentServiceException(Exception innerException)
            : base(message: "Failed teacher attachment service error occured.", innerException)
        {
        }
    }
}
