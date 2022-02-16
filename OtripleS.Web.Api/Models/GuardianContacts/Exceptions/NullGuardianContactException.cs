﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.GuardianContacts.Exceptions
{
    public class NullGuardianContactException : Exception
    {
        public NullGuardianContactException() : base(message: "The guardian contact is null.") { }
    }
}
