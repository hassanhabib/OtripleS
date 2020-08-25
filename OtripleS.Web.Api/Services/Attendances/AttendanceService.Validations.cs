// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;

namespace OtripleS.Web.Api.Services.Attendances
{
    public partial class AttendanceService 
    {
        private void ValidateAttendanceId(Guid attendanceId)
        {
            throw new InvalidAttendanceInputException(
                parameterName: nameof(Attendance.Id),
                parameterValue: attendanceId);
        }
    }
}
