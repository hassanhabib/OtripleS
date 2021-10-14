// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Teachers
{
    public partial class TeacherService
    {
        private static void ValidateTeacherId(Guid teacherId)
        {
            if (teacherId == default)
            {
                throw new InvalidTeacherException(
                    parameterName: nameof(Teacher.Id),
                    parameterValue: teacherId);
            }
        }

        private static void ValidateStorageTeacher(Teacher maybeTeacher, Guid teacherId)
        {
            if (maybeTeacher is null)
            {
                throw new NotFoundTeacherException(teacherId);
            }
        }

        private void ValidateStorageTeachers(IQueryable<Teacher> storageTeachers)
        {
            if (!storageTeachers.Any())
            {
                this.loggingBroker.LogWarning("No teachers found in storage.");
            }
        }

        private void ValidateTeacherOnCreate(Teacher teacher)
        {
            ValidateTeacher(teacher);

            Validate
            (
                (Rule: IsInvalidX(teacher.Id), Parameter: nameof(Teacher.Id)),
                (Rule: IsInvalidX(teacher.UserId), Parameter: nameof(Teacher.UserId)),
                (Rule: IsInvalidX(teacher.EmployeeNumber), Parameter: nameof(Teacher.EmployeeNumber)),
                (Rule: IsInvalidX(teacher.FirstName), Parameter: nameof(Teacher.FirstName)),
                (Rule: IsInvalidX(teacher.MiddleName), Parameter: nameof(Teacher.MiddleName)),
                (Rule: IsInvalidX(teacher.LastName), Parameter: nameof(Teacher.LastName)),
                (Rule: IsInvalidX(teacher.CreatedBy), Parameter: nameof(Teacher.CreatedBy)),
                (Rule: IsInvalidX(teacher.UpdatedBy), Parameter: nameof(Teacher.UpdatedBy)),
                (Rule: IsInvalidX(teacher.CreatedDate), Parameter: nameof(Teacher.CreatedDate)),
                (Rule: IsInvalidX(teacher.UpdatedDate), Parameter: nameof(Teacher.UpdatedDate))
            );

            ValidateUpdatedSignatureOnCreate(teacher);
            ValidateCreatedDateIsNotRecent(teacher);
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTeacherException = new InvalidTeacherException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTeacherException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTeacherException.ThrowIfContainsErrors();
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

        private void ValidateTeacherOnModify(Teacher teacher)
        {
            ValidateTeacher(teacher);
            ValidateTeacherId(teacher.Id);
            ValidateTeacherStrings(teacher);
            ValidateTeacherIds(teacher);
            ValidateTeacherDates(teacher);
            ValidateUpdatedSignatureOnUpdate(teacher);
        }

        private void ValidateCreatedDateIsNotRecent(Teacher teacher)
        {
            if (IsDateNotRecent(teacher.CreatedDate))
            {
                throw new InvalidTeacherException(
                    parameterName: nameof(teacher.CreatedDate),
                    parameterValue: teacher.CreatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateTeacherDates(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when teacher.CreatedDate == default:
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.CreatedDate),
                        parameterValue: teacher.CreatedDate);

                case { } when teacher.UpdatedDate == default:
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.UpdatedDate),
                        parameterValue: teacher.UpdatedDate);
            }
        }

        private static void ValidateTeacherIds(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when IsInvalid(teacher.CreatedBy):
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.CreatedBy),
                        parameterValue: teacher.CreatedBy);

                case { } when IsInvalid(teacher.UpdatedBy):
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.UpdatedBy),
                        parameterValue: teacher.UpdatedBy);
            }
        }

        private static void ValidateUpdatedSignatureOnCreate(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when teacher.CreatedBy != teacher.UpdatedBy:
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.UpdatedBy),
                        parameterValue: teacher.UpdatedBy);
            }
        }

        private void ValidateUpdatedSignatureOnUpdate(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when teacher.CreatedDate == teacher.UpdatedDate:
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.UpdatedDate),
                        parameterValue: teacher.UpdatedDate);

                case { } when IsDateNotRecent(teacher.UpdatedDate):
                    throw new InvalidTeacherException(
                        parameterName: nameof(teacher.UpdatedDate),
                        parameterValue: teacher.UpdatedDate);
            }
        }

        private static void ValidateTeacher(Teacher teacher)
        {
            if (teacher == default)
            {
                throw new NullTeacherException();
            }
        }

        private static void ValidateTeacherStrings(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when IsInvalid(teacher.UserId):
                    throw new InvalidTeacherException(
                        parameterName: nameof(teacher.UserId),
                        parameterValue: teacher.UserId);

                case { } when IsInvalid(teacher.EmployeeNumber):
                    throw new InvalidTeacherException(
                        parameterName: nameof(teacher.EmployeeNumber),
                        parameterValue: teacher.EmployeeNumber);

                case { } when IsInvalid(teacher.FirstName):
                    throw new InvalidTeacherException(
                        parameterName: nameof(teacher.FirstName),
                        parameterValue: teacher.FirstName);

                case { } when IsInvalid(teacher.LastName):
                    throw new InvalidTeacherException(
                        parameterName: nameof(teacher.LastName),
                        parameterValue: teacher.LastName);
            }
        }

        private static void ValidateAginstStorageTeacherOnModify(Teacher inputTeacher, Teacher storageTeacher)
        {
            switch (inputTeacher)
            {
                case { } when inputTeacher.CreatedDate != storageTeacher.CreatedDate:
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.CreatedDate),
                        parameterValue: inputTeacher.CreatedDate);

                case { } when inputTeacher.CreatedBy != storageTeacher.CreatedBy:
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.CreatedBy),
                        parameterValue: inputTeacher.CreatedBy);

                case { } when inputTeacher.UpdatedDate == storageTeacher.UpdatedDate:
                    throw new InvalidTeacherException(
                        parameterName: nameof(Teacher.UpdatedDate),
                        parameterValue: inputTeacher.UpdatedDate);
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
    }
}
