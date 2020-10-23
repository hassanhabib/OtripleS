// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Models.Guardians
{
    public class Guardian : IAuditable
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        [JsonIgnore]
        public IEnumerable<StudentGuardian> StudentGuardians { get; set; }

        [JsonIgnore]
        public IEnumerable<GuardianContact> GuardianContacts { get; set; }
    }
}
