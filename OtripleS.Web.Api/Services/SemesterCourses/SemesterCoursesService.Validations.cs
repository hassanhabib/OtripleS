// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
	public partial class SemesterCourseService
	{
        private void ValidateSemesterCourseOnCreate(SemesterCourse semesterCourse)
        {
            ValidateSemesterCourseIsNull(semesterCourse);
            ValidateSemesterCourseIdIsNull(semesterCourse.Id);
            ValidateSemesterCourseFields(semesterCourse);
        }

        private void ValidateSemesterCourseIsNull(SemesterCourse semesterCourse)
        {
            if (semesterCourse is null)
            {
                throw new NullSemesterCourseException();
            }
        }

        private void ValidateSemesterCourseId(Guid semesterCourseId)
        {
            if (semesterCourseId == Guid.Empty)
            {
                throw new InvalidSemesterCourseException(
                    parameterName: nameof(SemesterCourse.Id),
                    parameterValue: semesterCourseId);
            }
        }

        private void ValidateSemesterCourseFields(SemesterCourse semesterCourse)
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

        private static void ValidateStorageSemesterCourse(SemesterCourse storageSemesterCourse, Guid semesterCourseId)
        {
            if (storageSemesterCourse == null)
            {
                throw new NotFoundSemesterCourseException(semesterCourseId);
            }
        }

        private void ValidateSemesterCourseIdIsNull(Guid semesterCourseId)
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
    }
}
