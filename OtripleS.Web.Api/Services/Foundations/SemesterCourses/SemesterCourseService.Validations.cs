// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseService
    {
        private void ValidateSemesterCourseOnCreate(SemesterCourse semesterCourse)
        {
            ValidateSemesterCourseIsNull(semesterCourse);
            ValidateSemesterCourseIdIsNull(semesterCourse.Id);
            ValidateSemesterCourseFields(semesterCourse);
            ValidateInvalidAuditFields(semesterCourse);
            ValidateAuditFieldsDataOnCreate(semesterCourse);
        }

        private void ValidateSemesterCourseOnModify(SemesterCourse semesterCourse)
        {
            ValidateSemesterCourseIsNull(semesterCourse);
            ValidateSemesterCourseIdIsNull(semesterCourse.Id);
            ValidateSemesterCourseFields(semesterCourse);
            ValidateInvalidAuditFields(semesterCourse);
            ValidateAuditFieldsDataOnModify(semesterCourse);
        }

        private void ValidateStorageSemesterCourses(IQueryable<SemesterCourse> semesterCourses)
        {
            if (!semesterCourses.Any())
            {
                this.loggingBroker.LogWarning("No semesterSemesterCourses found in storage.");
            }
        }

        private static void ValidateSemesterCourseIsNull(SemesterCourse semesterCourse)
        {
            if (semesterCourse is null)
            {
                throw new NullSemesterCourseException();
            }
        }

        private static void ValidateSemesterCourseId(Guid semesterCourseId)
        {
            if (semesterCourseId == Guid.Empty)
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.Id),
                    parameterValue: semesterCourseId);
            }
        }

        private static void ValidateSemesterCourseFields(SemesterCourse semesterCourse)
        {
            if (IsInvalid(semesterCourse.StartDate))
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.StartDate),
                    parameterValue: semesterCourse.StartDate);
            }

            if (IsInvalid(semesterCourse.EndDate))
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.EndDate),
                    parameterValue: semesterCourse.EndDate);
            }

            if (IsInvalid(semesterCourse.CourseId))
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.CourseId),
                    parameterValue: semesterCourse.CourseId);
            }

            if (IsInvalid(semesterCourse.TeacherId))
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.TeacherId),
                    parameterValue: semesterCourse.TeacherId);
            }

            if (IsInvalid(semesterCourse.ClassroomId))
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.ClassroomId),
                    parameterValue: semesterCourse.ClassroomId);
            }
        }

        private static void ValidateInvalidAuditFields(SemesterCourse semesterCourse)
        {
            switch (semesterCourse)
            {
                case { } when IsInvalid(semesterCourse.CreatedBy):
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.CreatedBy),
                    parameterValue: semesterCourse.CreatedBy);

                case { } when IsInvalid(semesterCourse.UpdatedBy):
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.UpdatedBy),
                    parameterValue: semesterCourse.UpdatedBy);

                case { } when IsInvalid(semesterCourse.CreatedDate):
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.CreatedDate),
                    parameterValue: semesterCourse.CreatedDate);

                case { } when IsInvalid(semesterCourse.UpdatedDate):
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.UpdatedDate),
                    parameterValue: semesterCourse.UpdatedDate);
            }
        }

        private void ValidateAuditFieldsDataOnCreate(SemesterCourse semesterCourse)
        {
            switch (semesterCourse)
            {
                case { } when semesterCourse.UpdatedBy != semesterCourse.CreatedBy:
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.UpdatedBy),
                    parameterValue: semesterCourse.UpdatedBy);

                case { } when semesterCourse.UpdatedDate != semesterCourse.CreatedDate:
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.UpdatedDate),
                    parameterValue: semesterCourse.UpdatedDate);

                case { } when IsDateNotRecent(semesterCourse.CreatedDate):
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.CreatedDate),
                    parameterValue: semesterCourse.CreatedDate);
            }
        }

        private static void ValidateStorageSemesterCourse(SemesterCourse storageSemesterCourse, Guid semesterCourseId)
        {
            if (storageSemesterCourse is null)
            {
                throw new NotFoundSemesterCourseException(semesterCourseId);
            }
        }

        private static void ValidateSemesterCourseIdIsNull(Guid semesterCourseId)
        {
            if (semesterCourseId == default)
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.Id),
                    parameterValue: semesterCourseId);
            }
        }

        private static bool IsInvalid(DateTimeOffset input) => input == default;
        private static bool IsInvalid(Guid input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private void ValidateAuditFieldsDataOnModify(SemesterCourse semesterCourse)
        {
            switch (semesterCourse)
            {
                case { } when IsDateNotRecent(semesterCourse.UpdatedDate):
                    throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.UpdatedDate),
                    parameterValue: semesterCourse.UpdatedDate);
            }
        }

        private static void ValidateAgainstStorageSemesterCourseOnModify(SemesterCourse inputSemesterCourse, SemesterCourse storageSemesterCourse)
        {
            switch (inputSemesterCourse)
            {
                case { } when inputSemesterCourse.CreatedDate != storageSemesterCourse.CreatedDate:
                    throw new InvalidSemesterCourseException(
                        parameterName: nameof(SemesterCourse.CreatedDate),
                        parameterValue: inputSemesterCourse.CreatedDate);

                case { } when inputSemesterCourse.CreatedBy != storageSemesterCourse.CreatedBy:
                    throw new InvalidSemesterCourseException(
                        parameterName: nameof(SemesterCourse.CreatedBy),
                        parameterValue: inputSemesterCourse.CreatedBy);

                case { } when inputSemesterCourse.UpdatedDate == storageSemesterCourse.UpdatedDate:
                    throw new InvalidSemesterCourseException(
                        parameterName: nameof(SemesterCourse.UpdatedDate),
                        parameterValue: inputSemesterCourse.UpdatedDate);
            }
        }
    }
}
