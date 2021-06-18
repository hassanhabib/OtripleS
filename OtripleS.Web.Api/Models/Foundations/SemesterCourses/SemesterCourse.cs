// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OtripleS.Web.Api.Models.Foundations.Classrooms;
using OtripleS.Web.Api.Models.Foundations.Courses;
using OtripleS.Web.Api.Models.Foundations.Exams;
using OtripleS.Web.Api.Models.Foundations.StudentSemesterCourses;
using OtripleS.Web.Api.Models.Foundations.Teachers;

namespace OtripleS.Web.Api.Models.Foundations.SemesterCourses
{
    public class SemesterCourse : IAuditable
    {
        public Guid Id { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public SemesterCourseStatus Status { get; set; }

        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public Guid ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<StudentSemesterCourse> StudentSemesterCourses { get; set; }

        [JsonIgnore]
        public IEnumerable<Exam> Exams { get; set; }
    }
}
