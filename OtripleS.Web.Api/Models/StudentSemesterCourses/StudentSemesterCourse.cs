// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses
{
    public class StudentSemesterCourse
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid SemesterCourseId { get; set; }
        public SemesterCourse SemesterCourse { get; set; }

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
