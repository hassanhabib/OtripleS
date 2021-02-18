using System;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Models.ExamAttachments
{
    public class ExamAttachment {
        public Guid ExamId {get; set;}
        public Exam Exam {get; set;}
        public Guid AttachmentId {get; set;}
        public Attachment Attachment {get; set;}
        public string Notes {get; set;}
    }
}