﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentGuardians.Exceptions
{
    public class LockedStudentGuardianException : Exception
    {
        public LockedStudentGuardianException(Exception innerException)
            : base(message: "Locked student guardian record exception, please try again later.", innerException) { }
    }
}
