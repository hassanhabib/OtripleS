﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Attachments;

namespace OtripleS.Web.Api.Models.AssignmentAttachments
{
    public class AssignmentAttachment
    {
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public string Notes { get; set; }
    }
}
