// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Models.StudentExams
{
    public class StudentExam : IAuditable
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher ReviewingTeacher { get; set; }

        public double Score { get; set; }
        public StudentExamGrade Grade { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
