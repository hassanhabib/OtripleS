// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentAttachments.Exceptions
{
    public class LockedStudentAttachmentException : Exception
    {
        public LockedStudentAttachmentException(Exception innerException)
            : base(message: "Locked student attachment record exception, please try again later.", innerException)
        { }
    }
}
