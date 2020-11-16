// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.TeacherContacts;

namespace OtripleS.Web.Api.Models.Teachers
{
    public class Teacher : IAuditable
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public TeacherStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<SemesterCourse> SemesterCourses { get; set; }

        [JsonIgnore]
        public IEnumerable<TeacherContact> TeacherContacts { get; set; }

        [JsonIgnore]
        public IEnumerable<StudentExam> ReviewedStudentExams { get; set; }
    }
}
