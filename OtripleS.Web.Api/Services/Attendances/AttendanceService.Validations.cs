// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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

        private void ValidateStorageAttendances(IQueryable<Attendance> storageAttendances)
        {
            if (storageAttendances.Count() == 0)
            {
                this.loggingBroker.LogWarning("No Attendances found in storage.");
            }
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
                throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.Id),
                    parameterValue: attendanceId);
            }
        }

        private void ValidateUpdatedDateIsRecent(Attendance attendance)
        {
            if (IsDateNotRecent(attendance.UpdatedDate))
            {
                throw new InvalidAttendanceException(
                    parameterName: nameof(attendance.UpdatedDate),
                    parameterValue: attendance.UpdatedDate);
            }
        }

        private void ValidateStorageAttendance(Attendance storageAttendance, Guid attendanceId)
        {
            if (storageAttendance is null)
            {
                throw new NotFoundAttendanceException(attendanceId);
            }
        }

        private void ValidateAttendanceOnCreate(Attendance attendance)
        {
            ValidateAttendanceIsNull(attendance);
            ValidateMandatoryFields(attendance);
            ValidateAttendanceDatesOnAdd(attendance);
            ValidateAttendanceAuditFields(attendance);
        }

        private void ValidateAttendanceAuditFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsInvalid(attendance.CreatedBy):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.CreatedBy),
                        parameterValue: attendance.CreatedBy);

                case { } when IsInvalid(attendance.UpdatedBy):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.UpdatedBy),
                        parameterValue: attendance.UpdatedBy);

                case { } when IsInvalid(attendance.CreatedDate):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.CreatedDate),
                        parameterValue: attendance.CreatedDate);

                case { } when IsInvalid(attendance.UpdatedDate):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.UpdatedDate),
                        parameterValue: attendance.UpdatedDate);

                case { } when attendance.CreatedDate != attendance.UpdatedDate:
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

        private static void ValidateMandatoryFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsInvalid(attendance.Id):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.Id),
                        parameterValue: attendance.Id);

                case { } when IsInvalid(attendance.StudentSemesterCourseId):
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.StudentSemesterCourseId),
                        parameterValue: attendance.StudentSemesterCourseId);
            }
        }

        private void ValidateAgainstStorageAttendanceOnModify(Attendance inputAttendance, Attendance storageAttendance)
        {
            switch (inputAttendance)
            {
                case { } when inputAttendance.CreatedDate != storageAttendance.CreatedDate:
                    throw new InvalidAttendanceException(
                        parameterName: nameof(Attendance.CreatedDate),
                        parameterValue: inputAttendance.CreatedDate);

                case { } when inputAttendance.CreatedBy != storageAttendance.CreatedBy:
                    throw new InvalidAttendanceException(
                        parameterName: nameof(Attendance.CreatedBy),
                        parameterValue: inputAttendance.CreatedBy);

                case { } when inputAttendance.UpdatedDate == storageAttendance.UpdatedDate:
                    throw new InvalidAttendanceException(
                        parameterName: nameof(Attendance.UpdatedDate),
                        parameterValue: inputAttendance.UpdatedDate);
            }
        }

        private void ValidateDatesAreNotSame(Attendance attendance)
        {
            if (attendance.CreatedDate == attendance.UpdatedDate)
            {
                throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.UpdatedDate),
                    parameterValue: attendance.UpdatedDate);
            }
        }

        private void ValidateInvalidAuditFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when IsInvalid(attendance.CreatedBy):
                    throw new InvalidAttendanceException(
                    parameterName: nameof(attendance.CreatedBy),
                    parameterValue: attendance.CreatedBy);

                case { } when IsInvalid(attendance.CreatedDate):
                    throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.CreatedDate),
                    parameterValue: attendance.CreatedDate);

                case { } when IsInvalid(attendance.UpdatedBy):
                    throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.UpdatedBy),
                    parameterValue: attendance.UpdatedBy);

                case { } when IsInvalid(attendance.UpdatedDate):
                    throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.UpdatedDate),
                    parameterValue: attendance.UpdatedDate);
            }
        }
        private static bool IsInvalid(Guid inputId) => inputId == default;
        private static bool IsInvalid(DateTimeOffset inputDateTimeOffset) => inputDateTimeOffset == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
