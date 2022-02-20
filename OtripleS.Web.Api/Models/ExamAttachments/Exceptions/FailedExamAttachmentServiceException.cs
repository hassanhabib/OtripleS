// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class FailedExamAttachmentServiceException: Xeption
    {
        public FailedExamAttachmentServiceException(Exception innerException)
            : base(message: "Failed exam attachment service error occured, contact support",
                  innerException)
        { }
    }
}
