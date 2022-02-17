// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class FailedCalendarEntryAttachmentServiceException : Xeption
    {
        public FailedCalendarEntryAttachmentServiceException(Exception innerException)
            : base(message: "Failed calendar entry attachment service error occured,contact support", innerException)
        { }
    }
}
