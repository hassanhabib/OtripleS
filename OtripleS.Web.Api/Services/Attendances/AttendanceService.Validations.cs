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
        private static void ValidateAttendanceId(Guid attendanceId)
        {
            if (attendanceId == default)
            {
                throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.Id),
                    parameterValue: attendanceId);
            }
        }

        private static void ValidateAttendanceIsNotNull(Attendance inputAttendance)
        {
            if (inputAttendance is null)
            {
                throw new NullAttendanceException();
            }
        }

        private static void ValidateAttendanceOnCreate(Attendance attendance)
        {
            ValidateAttendanceIsNotNull(attendance);
            ValidateMandatoryFields(attendance);
        }

        private static void ValidateMandatoryFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsEmpty(attendance.Id):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.Id),
                        parameterValue: attendance.Id);

                case { } when IsEmpty(attendance.StudentSemesterCourseId):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.StudentSemesterCourseId),
                        parameterValue: attendance.StudentSemesterCourseId);
            }
        }

        private static bool IsEmpty(Guid id) => id == default;
    }
}
