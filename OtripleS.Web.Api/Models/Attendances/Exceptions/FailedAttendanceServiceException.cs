using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class FailedAttendanceServiceException :Xeption
    {
        public FailedAttendanceServiceException(Exception innerException)
            : base("Failed Attendance Service Exception, contact support", innerException)
        {}
    }
}
