// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Attendances
{
    public partial class AttendanceService
    {
        private void ValidateAttendanceOnCreate(Attendance attendance)
        {
            ValidateAttendanceIsNull(attendance);

            Validate(
                (Rule: IsInvalidX(attendance.Id), Parameter: nameof(Attendance.Id)),
                (Rule: IsInvalidX(attendance.StudentSemesterCourseId), Parameter: nameof(Attendance.StudentSemesterCourseId)),
                (Rule: IsInvalidX(attendance.AttendanceDate), Parameter: nameof(Attendance.AttendanceDate)),
                (Rule: IsNotRecent(attendance.AttendanceDate), Parameter: nameof(Attendance.AttendanceDate)),
                (Rule: IsInvalidX(attendance.Notes), Parameter: nameof(Attendance.Notes)),
                (Rule: IsInvalidX(attendance.CreatedBy), Parameter: nameof(Attendance.CreatedBy)),
                (Rule: IsInvalidX(attendance.UpdatedBy), Parameter: nameof(Attendance.UpdatedBy)),
                (Rule: IsInvalidX(attendance.CreatedDate), Parameter: nameof(Attendance.CreatedDate)),
                (Rule: IsInvalidX(attendance.UpdatedDate), Parameter: nameof(Attendance.UpdatedDate)),
                (Rule: IsNotRecent(attendance.CreatedDate), Parameter: nameof(Attendance.CreatedDate))
            );

            ValidateAttendanceAuditFields(attendance);
        }

        private void ValidateAttendanceOnModify(Attendance attendance)
        {
            ValidateAttendanceIsNull(attendance);
            ValidateAttendanceId(attendance.Id);
            ValidateInvalidAuditFields(attendance);
            ValidateDatesAreNotSame(attendance);
            ValidateUpdatedDateIsRecent(attendance);
        }

        private static dynamic IsInvalidX(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidX(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAttendanceException = new InvalidAttendanceException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAttendanceException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAttendanceException.ThrowIfContainsErrors();
        }

        private void ValidateStorageAttendances(IQueryable<Attendance> storageAttendances)
        {
            if (!storageAttendances.Any())
            {
                this.loggingBroker.LogWarning("No Attendances found in storage.");
            }
        }

        private static void ValidateAttendanceIsNull(Attendance attendance)
        {
            if (attendance is null)
            {
                throw new NullAttendanceException();
            }
        }

        private static void ValidateAttendanceId(Guid attendanceId)
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

        private static void ValidateStorageAttendance(Attendance storageAttendance, Guid attendanceId)
        {
            if (storageAttendance is null)
            {
                throw new NotFoundAttendanceException(attendanceId);
            }
        }

        private void ValidateAttendanceAuditFields(Attendance attendance)
        {
            switch (attendance)
            {
                case { } when attendance.CreatedDate != attendance.UpdatedDate:
                    throw new InvalidAttendanceException(
                        parameterName: nameof(attendance.UpdatedDate),
                        parameterValue: attendance.UpdatedDate);
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

        private static void ValidateAgainstStorageAttendanceOnModify(Attendance inputAttendance, Attendance storageAttendance)
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

        private static void ValidateDatesAreNotSame(Attendance attendance)
        {
            if (attendance.CreatedDate == attendance.UpdatedDate)
            {
                throw new InvalidAttendanceException(
                    parameterName: nameof(Attendance.UpdatedDate),
                    parameterValue: attendance.UpdatedDate);
            }
        }

        private static void ValidateInvalidAuditFields(Attendance attendance)
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
