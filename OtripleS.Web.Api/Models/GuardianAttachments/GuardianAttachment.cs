﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Guardians;

namespace OtripleS.Web.Api.Models.GuardianAttachments
{
    public class GuardianAttachment
    {
        public Guid GuardianId { get; set; }
        public Guardian Guardian { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
    }
}
