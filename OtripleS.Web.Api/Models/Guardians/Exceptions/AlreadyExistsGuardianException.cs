// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Guardians.Exceptions
{
    public class AlreadyExistsGuardianException : Exception
    {
        public AlreadyExistsGuardianException(Exception innerException)
            : base("Guardian with the same id already exists.", innerException) { }
    }
}
