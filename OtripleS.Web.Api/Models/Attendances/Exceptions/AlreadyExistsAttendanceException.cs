// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class AlreadyExistsAttendanceException : Exception
    {
        public AlreadyExistsAttendanceException(Exception innerException)
            : base("Attendance with the same id already exists.", innerException) { }
    }
}
