// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Foundations.Contacts;
using OtripleS.Web.Api.Models.Foundations.Guardians;

namespace OtripleS.Web.Api.Models.Foundations.GuardianContacts
{
    public class GuardianContact
    {
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public Guid GuardianId { get; set; }
        public Guardian Guardian { get; set; }
    }
}
