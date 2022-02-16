// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Calendars.Exceptions
{
    public class FailedCalendarServiceException : Xeption
    {
        public FailedCalendarServiceException(Exception innerException)
            : base(message: "Failed calendar service error occured, contact support", innerException)
        { }
    }
}
