﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Guardians.Exceptions
{
    public class FailedGuardianServiceException : Xeption
    {
        public FailedGuardianServiceException(Exception innerException)
            : base(message: "Failed guardian service error occurred, contact support.", innerException)
        {
        }
    }
}
