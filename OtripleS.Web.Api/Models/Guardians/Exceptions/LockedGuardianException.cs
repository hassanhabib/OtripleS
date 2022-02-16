﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Guardians.Exceptions
{
    public class LockedGuardianException : Exception
    {
        public LockedGuardianException(Exception innerException)
            : base(message: "Locked guardian record exception, please try again later.", innerException) { }
    }
}
