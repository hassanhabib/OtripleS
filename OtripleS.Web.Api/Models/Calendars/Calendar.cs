﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OtripleS.Web.Api.Models.CalendarEntries;

namespace OtripleS.Web.Api.Models.Calendars
{
    public class Calendar : IAuditable
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<CalendarEntry> CalendarEntries { get; set; }
    }
}
