// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;

namespace OtripleS.Web.Api.Tests.Acceptance.Models.TeacherContacts
{
    public class TeacherContact
    {
        public Guid ContactId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
