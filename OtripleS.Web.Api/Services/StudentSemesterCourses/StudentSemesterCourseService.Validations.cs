using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseService
    {
        private void ValidateStudentSemesterCourseOnCreate(StudentSemesterCourse studentSemesterCourse)
        {
            ValidateStudentSemesterCourseIsNull(studentSemesterCourse);
            ValidateStudentSemesterCourseIdIsNull(studentSemesterCourse.StudentId, studentSemesterCourse.SemesterCourseId);
            ValidateInvalidAuditFields(studentSemesterCourse);
            ValidateAuditFieldsDataOnCreate(studentSemesterCourse);
        }

        private void ValidateStudentSemesterCourseIsNull(StudentSemesterCourse studentSemesterCourse)
        {
            if (studentSemesterCourse is null)
            {
                throw new NullStudentSemesterCourseException();
            }
        }

        private void ValidateStudentSemesterCourseIdIsNull(Guid studentId, Guid semesterCourseId)
        {
            if (studentId == default)
            {
                throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.StudentId),
                    parameterValue: studentId);
            }

            if (semesterCourseId == default)
            {
                throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                    parameterValue: semesterCourseId);
            }
        }

        private void ValidateInvalidAuditFields(StudentSemesterCourse studentSemesterCourse)
        {
            switch (studentSemesterCourse)
            {
                case { } when IsInvalid(studentSemesterCourse.CreatedBy):
                    throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.CreatedBy),
                    parameterValue: studentSemesterCourse.CreatedBy);

                case { } when IsInvalid(studentSemesterCourse.UpdatedBy):
                    throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedBy),
                    parameterValue: studentSemesterCourse.UpdatedBy);

                case { } when IsInvalid(studentSemesterCourse.CreatedDate):
                    throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.CreatedDate),
                    parameterValue: studentSemesterCourse.CreatedDate);

                case { } when IsInvalid(studentSemesterCourse.UpdatedDate):
                    throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                    parameterValue: studentSemesterCourse.UpdatedDate);
            }
        }

        private static bool IsInvalid(DateTimeOffset input) => input == default;
        private static bool IsInvalid(Guid input) => input == default;

        private void ValidateAuditFieldsDataOnCreate(StudentSemesterCourse studentSemesterCourse)
        {
            switch (studentSemesterCourse)
            {
                case { } when studentSemesterCourse.UpdatedBy != studentSemesterCourse.CreatedBy:
                    throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedBy),
                    parameterValue: studentSemesterCourse.UpdatedBy);

                case { } when studentSemesterCourse.UpdatedDate != studentSemesterCourse.CreatedDate:
                    throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                    parameterValue: studentSemesterCourse.UpdatedDate);

                case { } when IsDateNotRecent(studentSemesterCourse.CreatedDate):
                    throw new InvalidStudentSemesterCourseException(
                    parameterName: nameof(StudentSemesterCourse.CreatedDate),
                    parameterValue: studentSemesterCourse.CreatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
