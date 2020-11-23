// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Tests.Acceptance.Models.Contacts
{
    public class Contact
    {
        public Guid Id { get; set; }
        public bool IsPrimary { get; set; }
        public ContactType Type { get; set; }
        public string Information { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
