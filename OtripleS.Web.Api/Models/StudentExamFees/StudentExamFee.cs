// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------
using System;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Models.StudentExamFees
{
    public class StudentExamFee : IAuditable
    {
        public Guid Id {get; set;}
        public Guid ExamFeeId {get; set;}
        public ExamFee ExamFee {get; set;}
        public Guid StudentId {get; set;}
        public Student Student {get; set;}
        public StudentExamFeeStatus Status {get; set;}
        public DateTimeOffset CreatedDate {get; set;}
        public DateTimeOffset UpdatedDate {get; set;}
        public Guid CreatedBy {get; set;}
        public User CreatedByUser {get; set;}
        public Guid UpdatedBy {get; set;}
        public User UpdatedByUser {get; set;}
    }
}
