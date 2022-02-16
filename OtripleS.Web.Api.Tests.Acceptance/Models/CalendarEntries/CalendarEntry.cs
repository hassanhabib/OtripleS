﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Tests.Acceptance.Models.CalendarEntries
{
    public class CalendarEntry
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public bool IsAllDay { get; set; }
        public DateTimeOffset RemindAtDateTime { get; set; }
        public RecurrenceType RecurrenceType { get; set; }
        public int Period { get; set; }
        public int RepeatCount { get; set; }
        public DateTimeOffset? RepeatUntil { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public Guid CalendarId { get; set; }
    }
}
