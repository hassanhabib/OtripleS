﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class LockedAttendanceException : Exception
    {
        public LockedAttendanceException(Exception innerException)
            : base(message: "Locked attendance record exception, please try again later.", innerException) { }
    }
}
