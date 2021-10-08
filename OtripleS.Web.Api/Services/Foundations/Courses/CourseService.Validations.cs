// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Courses
{
    public partial class CourseService
    {
        private void ValidateCourseOnCreate(Course course)
        {
            ValidateCourse(course);

            Validate(
                (Rule: IsInvalidX(course.Id), Parameter: nameof(Course.Id)),
                (Rule: IsInvalidX(text: course.Name), Parameter: nameof(Course.Name)),
                (Rule: IsInvalidX(text: course.Description), Parameter: nameof(Course.Description)),
                (Rule: IsInvalidX(course.CreatedDate), Parameter: nameof(Course.CreatedDate)),
                (Rule: IsInvalidX(course.UpdatedDate), Parameter: nameof(Course.UpdatedDate)),
                (Rule: IsInvalidX(id: course.CreatedBy), Parameter: nameof(Course.CreatedBy)),
                (Rule: IsInvalidX(id: course.UpdatedBy), Parameter: nameof(Course.UpdatedBy)),

                (Rule: IsNotSame(
                    firstId: course.UpdatedBy,
                    secondId: course.CreatedBy,
                    secondIdName: nameof(Course.CreatedBy)),
                Parameter: nameof(Course.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: course.UpdatedDate,
                    secondDate: course.CreatedDate,
                    secondDateName: nameof(Course.CreatedDate)),
                Parameter: nameof(Course.UpdatedDate)));

            ValidateCreatedSignature(course);
            ValidateCreatedDateIsRecent(course);
        }

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

        private static void ValidateCourse(Course course)
        {
            if (course is null)
            {
                throw new NullCourseException();
            }
        }

        private static dynamic IsInvalidX(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidX(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static void ValidateCourseId(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                throw new InvalidCourseException(
                    parameterName: nameof(Course.Id),
                    parameterValue: courseId);
            }
        }

        private static void ValidateCourseIds(Course course)
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

        private static void ValidateCourseStrings(Course course)
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

        private static void ValidateCourseDates(Course course)
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

        private static void ValidateCreatedSignature(Course course)
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

        private static void ValidateDatesAreNotSame(Course course)
        {
            if (course.CreatedDate == course.UpdatedDate)
            {
                throw new InvalidCourseException(
                    parameterName: nameof(Course.UpdatedDate),
                    parameterValue: course.UpdatedDate);
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

        private static void ValidateStorageCourse(Course storageCourse, Guid courseId)
        {
            if (storageCourse == null)
            {
                throw new NotFoundCourseException(courseId);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateStorageCourses(IQueryable<Course> storageCourses)
        {
            if (!storageCourses.Any())
            {
                this.loggingBroker.LogWarning("No courses found in storage.");
            }
        }

        private static void ValidateAgainstStorageCourseOnModify(Course inputCourse, Course storageCourse)
        {
            switch (inputCourse)
            {
                case { } when inputCourse.CreatedDate != storageCourse.CreatedDate:
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.CreatedDate),
                        parameterValue: inputCourse.CreatedDate);

                case { } when inputCourse.CreatedBy != storageCourse.CreatedBy:
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.CreatedBy),
                        parameterValue: inputCourse.CreatedBy);

                case { } when inputCourse.UpdatedDate == storageCourse.UpdatedDate:
                    throw new InvalidCourseException(
                        parameterName: nameof(Course.UpdatedDate),
                        parameterValue: inputCourse.UpdatedDate);
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCourseException = new InvalidCourseException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCourseException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCourseException.ThrowIfContainsErrors();
        }
    }
}
