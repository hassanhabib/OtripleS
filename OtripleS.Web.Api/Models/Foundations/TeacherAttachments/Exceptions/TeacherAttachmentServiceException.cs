//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Foundations.TeacherAttachments.Exceptions
{
    public class TeacherAttachmentServiceException : Exception
    {
        public TeacherAttachmentServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException)
        { }
    }
}
