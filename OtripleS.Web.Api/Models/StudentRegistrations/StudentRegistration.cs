﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Models.StudentRegistrations
{
    public class StudentRegistration
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public Guid RegistrationId { get; set; }
        public Registration Registration { get; set; }
    }
}
