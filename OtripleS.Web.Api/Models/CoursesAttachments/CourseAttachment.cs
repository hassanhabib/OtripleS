//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

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
