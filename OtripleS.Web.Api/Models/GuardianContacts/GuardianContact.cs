// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Guardians;

namespace OtripleS.Web.Api.Models.GuardianContacts
{
    public class GuardianContact
    {
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public Guid GuardianId { get; set; }
        public Guardian Guardian { get; set; }
    }
}
