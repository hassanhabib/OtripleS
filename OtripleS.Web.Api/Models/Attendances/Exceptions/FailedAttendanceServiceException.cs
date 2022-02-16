// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class FailedAttendanceServiceException : Xeption
    {
        public FailedAttendanceServiceException(Exception innerException)
            : base(message: "Failed attendance service exception, contact support.", innerException)
        { }
    }
}
