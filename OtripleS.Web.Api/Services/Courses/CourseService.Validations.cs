// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;

namespace OtripleS.Web.Api.Services.Courses
{
    public partial class CourseService
    {
        private void ValidateCourseOnModify(Course course)
        {
            ValidateCourse(course);
            ValidateCourseId(course.Id);
            ValidateCourseStrings(course);
            ValidateCourseIds(course);
            ValidateCourseDates(course);
            ValidateDatesAreNotSame(course);
            ValidateUpdatedDateIsRecent(course);
        }
        private void ValidateCourseId(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                throw new InvalidCourseInputException(
                    parameterName: nameof(Course.Id),
                    parameterValue: courseId);
            }
        }

        private void ValidateCourseStrings(Course course)
        {
            switch (course)
            {
                case { } when IsInvalid(course.Name):
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.Name),
                        parameterValue: course.Name);

                case { } when IsInvalid(course.Description):
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.Description),
                        parameterValue: course.Description);
            }
        }

        private void ValidateCourseIds(Course course)
        {
            switch (course)
            {
                case { } when IsInvalid(course.CreatedBy):
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.CreatedBy),
                        parameterValue: course.CreatedBy);

                case { } when IsInvalid(course.UpdatedBy):
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.UpdatedBy),
                        parameterValue: course.UpdatedBy);
            }
        }

        private void ValidateCourseDates(Course course)
        {
            switch (course)
            {
                case { } when course.CreatedDate == default:
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.CreatedDate),
                        parameterValue: course.CreatedDate);

                case { } when course.UpdatedDate == default:
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.UpdatedDate),
                        parameterValue: course.UpdatedDate);
            }
        }

        private void ValidateDatesAreNotSame(Course course)
        {
            if (course.CreatedDate == course.UpdatedDate)
            {
                throw new InvalidCourseInputException(
                    parameterName: nameof(Course.UpdatedDate),
                    parameterValue: course.UpdatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateUpdatedDateIsRecent(Course course)
        {
            if (IsDateNotRecent(course.UpdatedDate))
            {
                throw new InvalidCourseInputException(
                    parameterName: nameof(course.UpdatedDate),
                    parameterValue: course.UpdatedDate);
            }
        }

        private static void ValidateStorageCourse(Course storageCourse, Guid courseId)
        {
            if (storageCourse == null)
            {
                throw new NotFoundCourseException(courseId);
            }
        }

        private void ValidateCourse(Course course)
        {
            if (course is null)
            {
                throw new NullCourseException();
            }
        }

        private void ValidateAgainstStorageCourseOnModify(Course inputCourse, Course storageCourse)
        {
            switch (inputCourse)
            {
                case { } when inputCourse.CreatedDate != storageCourse.CreatedDate:
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.CreatedDate),
                        parameterValue: inputCourse.CreatedDate);

                case { } when inputCourse.CreatedBy != storageCourse.CreatedBy:
                    throw new InvalidCourseInputException(
                        parameterName: nameof(Course.CreatedBy),
                        parameterValue: inputCourse.CreatedBy);
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
    }
}
