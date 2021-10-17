// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Newtonsoft.Json;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.Users;
using System;
using System.Collections.Generic;

namespace OtripleS.Web.Api.Models.Registrations
{
    public class Registration : IAuditable
    {
        public Guid Id { get; set; }
        public RegistrationStatus Status { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterPhone { get; set; }
        public string SubmitterEmail { get; set; }
        public string StudentName { get; set; }
        public string StudentPhone { get; set; }
        public string StudentEmail { get; set; }
        public string AdditionalDetails { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public Guid CreatedBy { get; set; }
        public User CreatedByUser { get; set; }

        public Guid UpdatedBy { get; set; }
        public User UpdatedByUser { get; set; }

        [JsonIgnore]
        public IEnumerable<StudentRegistration> StudentRegistrations { get; set; }
    }
}
