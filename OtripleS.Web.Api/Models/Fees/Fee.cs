// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Models.Fees
{


    public class Fee : IAuditable
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public decimal Amount { get; set; }

        public Guid CreatedBy { get; set; }
        public User CreatedByUser { get; set; }

        public Guid UpdatedBy { get; set; }
        public User UpdatedByUser { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonIgnore]
        public IEnumerable<ExamFee> ExamFees { get; set; }
    }
}
