// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Foundations.Attachments;
using OtripleS.Web.Api.Models.Foundations.CalendarEntries;

namespace OtripleS.Web.Api.Models.Foundations.CalendarEntryAttachments
{
    public class CalendarEntryAttachment
    {
        public Guid CalendarEntryId { get; set; }
        public CalendarEntry CalendarEntry { get; set; }
        public Guid AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public string Notes { get; set; }
    }
}
