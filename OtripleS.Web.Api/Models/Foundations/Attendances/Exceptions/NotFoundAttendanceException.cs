// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class NotFoundAttendanceException : Exception
    {
        public NotFoundAttendanceException(Guid attendanceId)
            : base($"Couldn't find attendance with Id: {attendanceId}.") { }
    }
}
