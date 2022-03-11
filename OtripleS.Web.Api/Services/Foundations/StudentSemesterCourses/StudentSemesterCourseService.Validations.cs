// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentSemesterCourses
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

        private void ValidateStudentSemesterCourseOnModify(StudentSemesterCourse studentSemesterCourse)
        {
            ValidateStudentSemesterCourseIsNull(studentSemesterCourse);
            ValidateStudentSemesterCourseIdIsNull(studentSemesterCourse.StudentId, studentSemesterCourse.SemesterCourseId);
            ValidateStudentSemesterCourseFields(studentSemesterCourse);
            ValidateInvalidAuditFields(studentSemesterCourse);
            ValidateDatesAreNotSame(studentSemesterCourse);
            ValidateUpdatedDateIsRecent(studentSemesterCourse);
        }

        private void ValidateUpdatedDateIsRecent(StudentSemesterCourse studentSemesterCourse)
        {
            if (IsDateNotRecent(studentSemesterCourse.UpdatedDate))
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                    parameterValue: studentSemesterCourse.UpdatedDate);
            }
        }

        private static void ValidateDatesAreNotSame(StudentSemesterCourse studentSemesterCourse)
        {
            if (studentSemesterCourse.CreatedDate == studentSemesterCourse.UpdatedDate)
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                    parameterValue: studentSemesterCourse.UpdatedDate);
            }
        }

        private static void ValidateStudentSemesterCourseFields(StudentSemesterCourse studentSemesterCourse)
        {
            if (IsInvalid(studentSemesterCourse.Grade))
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.Grade),
                    parameterValue: studentSemesterCourse.Grade);
            }

            if (IsInvalid(studentSemesterCourse.Repeats))
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.Repeats),
                    parameterValue: studentSemesterCourse.Repeats);
            }
        }

        private static void ValidateStudentSemesterCourseIsNull(StudentSemesterCourse studentSemesterCourse)
        {
            if (studentSemesterCourse is null)
            {
                throw new NullStudentSemesterCourseException();
            }
        }

        private static void ValidateStudentSemesterCourseIdIsNull(Guid studentId, Guid semesterCourseId)
        {
            if (studentId == default)
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.StudentId),
                    parameterValue: studentId);
            }

            if (semesterCourseId == default)
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                    parameterValue: semesterCourseId);
            }
        }

        private static void ValidateInvalidAuditFields(StudentSemesterCourse studentSemesterCourse)
        {
            switch (studentSemesterCourse)
            {
                case { } when IsInvalid(studentSemesterCourse.CreatedBy):
                    throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.CreatedBy),
                    parameterValue: studentSemesterCourse.CreatedBy);

                case { } when IsInvalid(studentSemesterCourse.UpdatedBy):
                    throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedBy),
                    parameterValue: studentSemesterCourse.UpdatedBy);

                case { } when IsInvalid(studentSemesterCourse.CreatedDate):
                    throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.CreatedDate),
                    parameterValue: studentSemesterCourse.CreatedDate);

                case { } when IsInvalid(studentSemesterCourse.UpdatedDate):
                    throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                    parameterValue: studentSemesterCourse.UpdatedDate);
            }
        }

        private static bool IsInvalid(DateTimeOffset input) => input == default;
        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(int input) => input <= 0;

        private void ValidateAuditFieldsDataOnCreate(StudentSemesterCourse studentSemesterCourse)
        {
            switch (studentSemesterCourse)
            {
                case { } when studentSemesterCourse.UpdatedBy != studentSemesterCourse.CreatedBy:
                    throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedBy),
                    parameterValue: studentSemesterCourse.UpdatedBy);

                case { } when studentSemesterCourse.UpdatedDate != studentSemesterCourse.CreatedDate:
                    throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                    parameterValue: studentSemesterCourse.UpdatedDate);

                case { } when IsDateNotRecent(studentSemesterCourse.CreatedDate):
                    throw new InvalidStudentSemesterCourseInputException(
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

        private static void ValidateAgainstStorageStudentSemesterCourseOnModify
            (StudentSemesterCourse inputStudentSemesterCourse, StudentSemesterCourse storageStudentSemesterCourse)
        {
            switch (inputStudentSemesterCourse)
            {
                case { } when inputStudentSemesterCourse.CreatedDate != storageStudentSemesterCourse.CreatedDate:
                    throw new InvalidStudentSemesterCourseInputException(
                        parameterName: nameof(StudentSemesterCourse.CreatedDate),
                        parameterValue: inputStudentSemesterCourse.CreatedDate);
            }
        }

        private static void ValidateSemesterCourseId(Guid semesterCourseId)
        {
            if (semesterCourseId == Guid.Empty)
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                    parameterValue: semesterCourseId);
            }
        }

        private static void ValidateStudentId(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new InvalidStudentSemesterCourseInputException(
                    parameterName: nameof(StudentSemesterCourse.StudentId),
                    parameterValue: studentId);
            }
        }

        private static void ValidateStorageStudentSemesterCourse(
            StudentSemesterCourse storageStudentSemesterCourse,
            Guid semesterCourseId, Guid studentId)
        {
            if (storageStudentSemesterCourse is null)
            {
                throw new NotFoundStudentSemesterCourseException(semesterCourseId, studentId);
            }
        }
    }
}
