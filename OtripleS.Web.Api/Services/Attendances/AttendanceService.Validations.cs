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

        private void ValidateAttendanceOnCreate(Attendance attendance)
        {
            ValidateAttendanceIsNotNull(attendance);
            ValidateMandatoryFields(attendance);
            ValidateAttendanceDatesOnAdd(attendance);
            ValidateAttendanceAuditFields(attendance);
        }

        private void ValidateAttendanceAuditFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsValid(attendance.CreatedBy):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.CreatedBy),
                        parameterValue: attendance.CreatedBy);

                case { } when IsValid(attendance.UpdatedBy):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.UpdatedBy),
                        parameterValue: attendance.UpdatedBy);

                case { } when IsValid(attendance.UpdatedDate):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.UpdatedDate),
                        parameterValue: attendance.UpdatedDate);

                case { } when IsDateNotRecent(attendance.CreatedDate):
                    throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.CreatedDate),
                    parameterValue: attendance.CreatedDate);
            }
        }

        private void ValidateAttendanceDatesOnAdd(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsDateNotRecent(attendance.AttendanceDate):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.AttendanceDate),
                        parameterValue: attendance.AttendanceDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateMandatoryFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsValid(attendance.Id):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.Id),
                        parameterValue: attendance.Id);

                case { } when IsValid(attendance.StudentSemesterCourseId):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.StudentSemesterCourseId),
                        parameterValue: attendance.StudentSemesterCourseId);
            }
        }

        private static bool IsValid(Guid id) => id == default;
        private static bool IsValid(DateTimeOffset dateTimeOffset) => dateTimeOffset == default;
    }
}
