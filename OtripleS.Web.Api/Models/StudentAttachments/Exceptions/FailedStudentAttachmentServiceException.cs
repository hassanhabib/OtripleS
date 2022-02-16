// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.StudentAttachments.Exceptions
{
    public class FailedStudentAttachmentServiceException : Xeption
    {
        public FailedStudentAttachmentServiceException(Exception innerException)
            : base(message:"Failed student attachment service error occured,contact support.",innerException)
        {
        }
    }
}
