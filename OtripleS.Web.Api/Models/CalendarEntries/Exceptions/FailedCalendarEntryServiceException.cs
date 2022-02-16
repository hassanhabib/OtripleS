﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class FailedCalendarEntryServiceException : Xeption
    {
        public FailedCalendarEntryServiceException(Exception innerException)
            : base(message: "Failed calendar entry service error occured, contact support.", innerException)
        { }
    }
}
