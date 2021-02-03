// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Models.Attachments
{
    public class Attachment : IAuditable
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public byte[] Contents { get; set; }
        public string ContectType { get; set; }
        public string Extension { get; set; }
        public string ExternalUrl { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<StudentAttachment> StudentAttachments { get; set; }
        
        [JsonIgnore]
        public IEnumerable<GuardianAttachment> GuardianAttachments { get; set; }
        
        [JsonIgnore]
        public IEnumerable<TeacherAttachment> TeacherAttachments { get; set; }

        [JsonIgnore]
        public IEnumerable<CalendarEntryAttachment> CalendarEntryAttachments { get; set; }

        [JsonIgnore]
        public IEnumerable<CourseAttachment> CourseAttachments { get; set; }
    }
}
