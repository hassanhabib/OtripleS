// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.SemesterCourses;

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
    }
}
