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
                (Rule: IsNotRecent(attendance.CreatedDate), Parameter: nameof(Attendance.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: attendance.UpdatedDate,
                    secondDate: attendance.CreatedDate,
                    secondDateName: nameof(Attendance.CreatedDate)),
                Parameter: nameof(Attendance.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: attendance.UpdatedBy,
                    secondId: attendance.CreatedBy,
                    secondIdName: nameof(Attendance.CreatedBy)),
                Parameter: nameof(Attendance.UpdatedBy))
            );
        }

        private void ValidateAttendanceOnModify(Attendance attendance)
        {
            ValidateAttendanceIsNull(attendance);

            Validate(
                (Rule: IsInvalidX(attendance.Id), Parameter: nameof(Attendance.Id)),
                (Rule: IsInvalidX(attendance.StudentSemesterCourseId), Parameter: nameof(Attendance.StudentSemesterCourseId)),
                (Rule: IsInvalidX(attendance.AttendanceDate), Parameter: nameof(Attendance.AttendanceDate)),
                (Rule: IsInvalidX(attendance.Notes), Parameter: nameof(Attendance.Notes)),
                (Rule: IsInvalidX(attendance.CreatedBy), Parameter: nameof(Attendance.CreatedBy)),
                (Rule: IsInvalidX(attendance.UpdatedBy), Parameter: nameof(Attendance.UpdatedBy)),
                (Rule: IsInvalidX(attendance.CreatedDate), Parameter: nameof(Attendance.CreatedDate)),
                (Rule: IsInvalidX(attendance.UpdatedDate), Parameter: nameof(Attendance.UpdatedDate)),
                (Rule: IsNotRecent(attendance.UpdatedDate), Parameter: nameof(Attendance.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: attendance.UpdatedDate,
                    secondDate: attendance.CreatedDate,
                    secondDateName: nameof(Attendance.CreatedDate)),
                Parameter: nameof(Attendance.UpdatedDate))
            );
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
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

        private static void ValidateStorageAttendance(Attendance storageAttendance, Guid attendanceId)
        {
            if (storageAttendance is null)
            {
                throw new NotFoundAttendanceException(attendanceId);
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
