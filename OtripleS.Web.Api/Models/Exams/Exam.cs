// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.StudentExams;

namespace OtripleS.Web.Api.Models.Exams
{
    public class Exam : IAuditable
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Content { get; set; }
        public string Notes { get; set; }
        public ExamType Type { get; set; }

        public Guid SemesterCourseId { get; set; }
        public SemesterCourse SemesterCourse { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<StudentExam> StudentExams { get; set; }
    }
}
