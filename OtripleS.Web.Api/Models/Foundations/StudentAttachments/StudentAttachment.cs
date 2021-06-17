// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Models.Foundations.StudentAttachments
{
    public class StudentAttachment
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }

        public string Notes { get; set; }
    }
}
