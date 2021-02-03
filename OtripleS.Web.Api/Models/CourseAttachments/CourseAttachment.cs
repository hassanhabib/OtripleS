using System;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Models.CourseAttachments
{
    public class CourseAttachment
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public string Notes { get; set; }

    }
}
