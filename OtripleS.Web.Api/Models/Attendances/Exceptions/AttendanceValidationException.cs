// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class AttendanceValidationException : Exception
    {
        public AttendanceValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
