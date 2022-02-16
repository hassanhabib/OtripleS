﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Guardians.Exceptions
{
    public class NotFoundGuardianException : Exception
    {
        public NotFoundGuardianException(Guid guardianId)
            : base(message: $"Couldn't find guardian with id: {guardianId}.") { }
    }
}
