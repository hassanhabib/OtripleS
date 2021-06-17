// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.CalendarEntriesAttachments
{
    public class CalendarEntryAttachment
    {
        public Guid CalendarEntryId { get; set; }
        public Guid AttachmentId { get; set; }
        public string Notes { get; set; }
    }
}
