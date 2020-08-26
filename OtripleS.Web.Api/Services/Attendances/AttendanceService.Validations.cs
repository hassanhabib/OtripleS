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
        private void ValidateAttendanceOnModify(Attendance attendance)
        {
            ValidateAttendanceIsNull(attendance);
            ValidateAttendanceId(attendance.Id);
            ValidateInvalidAuditFields(attendance);
            ValidateDatesAreNotSame(attendance);
            ValidateUpdatedDateIsRecent(attendance);
        }

        private void ValidateAttendanceIsNull(Attendance attendance)
        {
            if (attendance is null)
            {
                throw new NullAttendanceException();
            }
        }

        private void ValidateAttendanceId(Guid attendanceId)
        {
            if (attendanceId == default)
            {
                throw new InvalidAttendanceInputException(
                    parameterName: nameof(Attendance.Id),
                    parameterValue: attendanceId);
            }
        }

        private void ValidateUpdatedDateIsRecent(Attendance attendance)
        {
            if (IsDateNotRecent(attendance.UpdatedDate))
            {
                throw new InvalidAttendanceInputException(
                    parameterName: nameof(attendance.UpdatedDate),
                    parameterValue: attendance.UpdatedDate);
            }
        }

        private void ValidateStorageAttendance(Attendance storageAttendance)
        {
            if (storageAttendance is null)
            {
                throw new NullAttendanceException();
            }
        }

        private void ValidateStorageAttendance(Attendance storageAttendance, Guid attendanceId)
        {
            if (storageAttendance == null)
            {
                throw new NotFoundAttendanceException(attendanceId);
            }
        }

        private void ValidateAgainstStorageAttendanceOnModify(Attendance inputAttendance, Attendance storageAttendance)
        {
            switch (inputAttendance)
            {
                case { } when inputAttendance.CreatedDate != storageAttendance.CreatedDate:
                    throw new InvalidAttendanceInputException(
                        parameterName: nameof(Attendance.CreatedDate),
                        parameterValue: inputAttendance.CreatedDate);

                case { } when inputAttendance.CreatedBy != storageAttendance.CreatedBy:
                    throw new InvalidAttendanceInputException(
                        parameterName: nameof(Attendance.CreatedBy),
                        parameterValue: inputAttendance.CreatedBy);
            }
        }

        private void ValidateDatesAreNotSame(Attendance attendance)
        {
            if (attendance.CreatedDate == attendance.UpdatedDate)
            {
                throw new InvalidAttendanceInputException(
                    parameterName: nameof(Attendance.UpdatedDate),
                    parameterValue: attendance.UpdatedDate);
            }
        }

        private void ValidateInvalidAuditFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsInvalid(attendance.CreatedBy):
                    throw new InvalidAttendanceInputException(
                    parameterName: nameof(attendance.CreatedBy),
                    parameterValue: attendance.CreatedBy);

                case { } when IsInvalid(attendance.CreatedDate):
                    throw new InvalidAttendanceInputException(
                    parameterName: nameof(Attendance.CreatedDate),
                    parameterValue: attendance.CreatedDate);

                case { } when IsInvalid(attendance.UpdatedBy):
                    throw new InvalidAttendanceInputException(
                    parameterName: nameof(Attendance.UpdatedBy),
                    parameterValue: attendance.UpdatedBy);

                case { } when IsInvalid(attendance.UpdatedDate):
                    throw new InvalidAttendanceInputException(
                    parameterName: nameof(Attendance.UpdatedDate),
                    parameterValue: attendance.UpdatedDate);
            }
        }
        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;
        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
