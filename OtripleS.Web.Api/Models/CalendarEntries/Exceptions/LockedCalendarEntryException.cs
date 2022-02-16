﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.CalendarEntries.Exceptions
{
    public class LockedCalendarEntryException : Exception
    {
        public LockedCalendarEntryException(Exception innerException)
            : base(message: "Locked calendar entry record exception, please try again later.", innerException) { }
    }
}
