// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Classrooms
{
    public partial class ClassroomService
    {
        private void ValidateClassroomOnCreate(Classroom classroom)
        {
            ValidateClassroomIsNull(classroom);

            Validate
            (
                (Rule: IsInvalidX(classroom.Id), Parameter: nameof(Classroom.Id)),
                (Rule: IsInvalidX(classroom.Name), Parameter: nameof(Classroom.Name)),
                (Rule: IsInvalidX(classroom.Location), Parameter: nameof(Classroom.Location)),
                (Rule: IsInvalidX(classroom.Status), Parameter: nameof(Classroom.Status)),
                (Rule: IsInvalidX(classroom.CreatedBy), Parameter: nameof(Classroom.CreatedBy)),
                (Rule: IsInvalidX(classroom.UpdatedBy), Parameter: nameof(Classroom.UpdatedBy)),
                (Rule: IsInvalidX(classroom.CreatedDate), Parameter: nameof(Classroom.CreatedDate)),
                (Rule: IsInvalidX(classroom.UpdatedDate), Parameter: nameof(Classroom.UpdatedDate)),
                (Rule: IsNotRecent(classroom.CreatedDate), Parameter: nameof(Classroom.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: classroom.UpdatedBy,
                    secondId: classroom.CreatedBy,
                    secondIdName: nameof(Classroom.CreatedBy)),
                Parameter: nameof(Classroom.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: classroom.UpdatedDate,
                    secondDate: classroom.CreatedDate,
                    secondDateName: nameof(Classroom.CreatedDate)),
                Parameter: nameof(Classroom.UpdatedDate))

            );

        }

        private void ValidateClassroomOnModify(Classroom classroom)
        {
            ValidateClassroomIsNull(classroom);
            Validate
            (
                (Rule: IsInvalidX(classroom.Id), Parameter: nameof(Classroom.Id)),
                (Rule: IsInvalidX(classroom.Name), Parameter: nameof(Classroom.Name)),
                (Rule: IsInvalidX(classroom.Location), Parameter: nameof(Classroom.Location)),
                (Rule: IsInvalidX(classroom.CreatedBy), Parameter: nameof(Classroom.CreatedBy)),
                (Rule: IsInvalidX(classroom.UpdatedBy), Parameter: nameof(Classroom.UpdatedBy)),
                (Rule: IsInvalidX(classroom.CreatedDate), Parameter: nameof(Classroom.CreatedDate)),
                (Rule: IsInvalidX(classroom.UpdatedDate), Parameter: nameof(Classroom.UpdatedDate)),
               (Rule: IsNotRecent(classroom.UpdatedDate), Parameter: nameof(Classroom.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: classroom.UpdatedDate,
                    secondDate: classroom.CreatedDate,
                    secondDateName: nameof(Classroom.CreatedDate)),
                Parameter: nameof(Classroom.UpdatedDate))

            );
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidClassroomException = new InvalidClassroomException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidClassroomException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidClassroomException.ThrowIfContainsErrors();
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
        private static dynamic IsInvalidX(ClassroomStatus status) => new
        {
            Condition = status != ClassroomStatus.Available,
            Message = "Value is invalid"
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

        private static void ValidateClassroomIsNull(Classroom classroom)
        {
            if (classroom is null)
            {
                throw new NullClassroomException();
            }
        }

        private static void ValidateClassroomIdIsNull(Guid classroomId)
        {
            if (classroomId == default)
            {
                throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.Id),
                    parameterValue: classroomId);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateStorageClassroom(Classroom storageClassroom, Guid classroomId)
        {
            if (storageClassroom == null)
            {
                throw new NotFoundClassroomException(classroomId);
            }
        }

        private static void ValidateAgainstStorageClassroomOnModify(Classroom inputClassroom, Classroom storageClassroom)
        {
            switch (inputClassroom)
            {
                case { } when inputClassroom.CreatedDate != storageClassroom.CreatedDate:
                    throw new InvalidClassroomException(
                        parameterName: nameof(Classroom.CreatedDate),
                        parameterValue: inputClassroom.CreatedDate);

                case { } when inputClassroom.CreatedBy != storageClassroom.CreatedBy:
                    throw new InvalidClassroomException(
                        parameterName: nameof(Classroom.CreatedBy),
                        parameterValue: inputClassroom.CreatedBy);

                case { } when inputClassroom.UpdatedDate == storageClassroom.UpdatedDate:
                    throw new InvalidClassroomException(
                        parameterName: nameof(Classroom.UpdatedDate),
                        parameterValue: inputClassroom.UpdatedDate);
            }
        }

        private void ValidateStorageClassrooms(IQueryable<Classroom> storageClassrooms)
        {
            if (!storageClassrooms.Any())
            {
                this.loggingBroker.LogWarning("No classrooms found in storage.");
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;
    }
}
