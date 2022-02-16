﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class CalendarEntryServiceException : Exception
    {
        public CalendarEntryServiceException(Exception innerException)
            : base(message: "Service error occurred, contact support.", innerException) { }
    }
}
