// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Tests.Acceptance.Models.StudentSemesterCourses
{
    public class StudentSemesterCourse
    {
        public Guid StudentId { get; set; }
        public Guid SemesterCourseId { get; set; }

        public string Grade { get; set; }
        public double Score { get; set; }
        public int Repeats { get; set; }
        public StudentSemesterCourseStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
