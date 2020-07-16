using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Courses
{
    public partial class CourseService
    {
        private void ValidateCourseOnCreate(Course course)
        {
            ValidateCourse(course);
            ValidateCourseId(course.Id);
            ValidateCourseIds(course);
            ValidateCourseStrings(course);
            ValidateCourseDates(course);
            ValidateCreatedSignature(course);
            ValidateCreatedDateIsRecent(course);
        }

        private void ValidateCourse(Course course)
        {
            if (course is null)
            {
                throw new NullCourseException();
            }
        }

        private void ValidateCourseId(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                throw new InvalidCourseException(
                    parameterName: nameof(Course.Id),
                    parameterValue: courseId);
            }
        }

        private void ValidateCourseIds(Course course)
        {
            switch (course)
            {
                case { } when IsInvalid(course.CreatedBy):
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.CreatedBy),
                        parameterValue: course.CreatedBy);

                case { } when IsInvalid(course.UpdatedBy):
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.UpdatedBy),
                        parameterValue: course.UpdatedBy);
            }
        }

        private void ValidateCourseStrings(Course course)
        {
            switch (course)
            {
                case { } when IsInvalid(course.Name):
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.Name),
                        parameterValue: course.Name);

                case { } when IsInvalid(course.Description):
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.Description),
                        parameterValue: course.Description);
            }
        }

        private void ValidateCourseDates(Course course)
        {
            switch (course)
            {
                case { } when course.CreatedDate == default:
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.CreatedDate),
                        parameterValue: course.CreatedDate);

                case { } when course.UpdatedDate == default:
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.UpdatedDate),
                        parameterValue: course.UpdatedDate);
            }
        }

        private void ValidateCreatedSignature(Course course)
        {
            if (course.CreatedBy != course.UpdatedBy)
            {
                throw new InvalidCourseException(
                    parameterName: nameof(Course.UpdatedBy),
                    parameterValue: course.UpdatedBy);
            }
            else if (course.CreatedDate != course.UpdatedDate)
            {
                throw new InvalidCourseException(
                    parameterName: nameof(Course.UpdatedDate),
                    parameterValue: course.UpdatedDate);
            }
        }
        private void ValidateCreatedDateIsRecent(Course course)
        {
            if (IsDateNotRecent(course.CreatedDate))
            {
                throw new InvalidCourseException(
                    parameterName: nameof(course.CreatedDate),
                    parameterValue: course.CreatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Course course)
        {
            if (IsDateNotRecent(course.UpdatedDate))
            {
                throw new InvalidCourseException(
                    parameterName: nameof(course.UpdatedDate),
                    parameterValue: course.UpdatedDate);
            }
        }

        private void ValidateDatesAreNotSame(Course course)
        {
            if (course.CreatedDate == course.UpdatedDate)
            {
                throw new InvalidCourseException(
                    parameterName: nameof(Course.CreatedDate),
                    parameterValue: course.CreatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
    }
}
