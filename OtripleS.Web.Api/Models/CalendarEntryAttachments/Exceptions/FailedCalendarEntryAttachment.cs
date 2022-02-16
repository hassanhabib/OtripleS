// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions
{
    public class FailedCalendarEntryAttachment : Xeption
    {
        public FailedCalendarEntryAttachment(Exception innerException)
            :base(message:"Failed calendar entry attachment error occured, contact support.", innerException)
        { }
    }
}
