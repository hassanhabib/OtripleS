// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Fees;
using System.Collections.Generic;
using Newtonsoft.Json;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Models.ExamFees
{
    public class ExamFee : IAuditable
    {
        public Guid Id {get; set;}
        public Guid ExamId {get; set;}
        public Exam Exam {get;set;}
        public Guid FeeId {get; set;}
        public Fee Fee {get;set;}
        public ExamFeeStatus Status {get; set;}
        public DateTimeOffset CreatedDate {get; set;}
        public DateTimeOffset UpdatedDate {get; set;}
        public Guid CreatedBy {get; set;}
        public User CreatedByUser {get; set;}
        public Guid UpdatedBy {get; set;}
        public User UpdatedByUser {get; set;}

        [JsonIgnore]
        public IEnumerable<StudentExamFee> StudentExamFees { get; set; }
    }
}
