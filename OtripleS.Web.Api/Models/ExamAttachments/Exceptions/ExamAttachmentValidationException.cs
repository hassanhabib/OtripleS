using System;
namespace OtripleS.Web.Api.Models.ExamAttachments.Exceptions
{
    public class ExamAttachmentValidationException : Exception
    {
        public ExamAttachmentValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
