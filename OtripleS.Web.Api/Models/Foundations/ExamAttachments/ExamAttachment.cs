// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Models.Foundations.ExamAttachments
{
    public class ExamAttachment
    {
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public string Notes { get; set; }
    }
}