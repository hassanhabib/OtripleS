﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Teachers
{
    public partial class TeacherService
    {
        private static void ValidateTeacherId(Guid teacherId)
        {
            Validate((Rule: IsInvalidX(teacherId), Parameter: nameof(Teacher.Id)));
        }

        private static void ValidateStorageTeacher(Teacher maybeTeacher, Guid teacherId)
        {
            if (maybeTeacher is null)
            {
                throw new NotFoundTeacherException(teacherId);
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
                (Rule: IsInvalidX(teacher.Status), Parameter: nameof(Teacher.Status)),
                (Rule: IsInvalidX(teacher.CreatedBy), Parameter: nameof(Teacher.CreatedBy)),
                (Rule: IsInvalidX(teacher.UpdatedBy), Parameter: nameof(Teacher.UpdatedBy)),
                (Rule: IsInvalidX(teacher.CreatedDate), Parameter: nameof(Teacher.CreatedDate)),
                (Rule: IsInvalidX(teacher.UpdatedDate), Parameter: nameof(Teacher.UpdatedDate)),
                (Rule: IsNotRecent(teacher.CreatedDate), Parameter: nameof(Teacher.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: teacher.UpdatedBy,
                    secondId: teacher.CreatedBy,
                    secondIdName: nameof(Teacher.CreatedBy)),
                Parameter: nameof(Teacher.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: teacher.UpdatedDate,
                    secondDate: teacher.CreatedDate,
                    secondDateName: nameof(Teacher.CreatedDate)),
                Parameter: nameof(Teacher.UpdatedDate))
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

        private static dynamic IsInvalidX(TeacherStatus status) => new
        {
            Condition = status != TeacherStatus.Active,
            Message = "Value is invalid"
        };

        private static dynamic IsInvalidX(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
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

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private void ValidateTeacherOnModify(Teacher teacher)
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
                (Rule: IsInvalidX(teacher.UpdatedDate), Parameter: nameof(Teacher.UpdatedDate)),
                (Rule: IsNotRecent(teacher.UpdatedDate), Parameter: nameof(Teacher.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: teacher.UpdatedDate,
                    secondDate: teacher.CreatedDate,
                    secondDateName: nameof(Teacher.CreatedDate)),

                Parameter: nameof(Teacher.UpdatedDate))
            );
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void ValidateTeacher(Teacher teacher)
        {
            if (teacher is null)
            {
                throw new NullTeacherException();
            }
        }

        private static void ValidateAgainstStorageTeacherOnModify(Teacher inputTeacher, Teacher storageTeacher)
        {
            Validate(
                (Rule: IsNotSame(
                    firstId: inputTeacher.CreatedBy,
                    secondId: storageTeacher.CreatedBy,
                    secondIdName: nameof(Teacher.CreatedBy)),
                Parameter: nameof(Teacher.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: inputTeacher.CreatedDate,
                    secondDate: storageTeacher.CreatedDate,
                    secondDateName: nameof(Teacher.CreatedDate)),
                Parameter: nameof(Teacher.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputTeacher.UpdatedDate,
                    secondDate: storageTeacher.UpdatedDate,
                    secondDateName: nameof(Teacher.UpdatedDate)),
                Parameter: nameof(Teacher.UpdatedDate)));
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;

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
    }
}