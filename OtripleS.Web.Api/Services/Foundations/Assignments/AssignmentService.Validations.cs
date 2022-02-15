// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Assignments
{
    public partial class AssignmentService
    {
        private void ValidateAssignmentOnCreate(Assignment assignment)
        {
            ValidateAssignmentIsNull(assignment);

            Validate(
                (Rule: IsInvalidX(assignment.Id), Parameter: nameof(Assignment.Id)),
                (Rule: IsInvalidX(assignment.Label), Parameter: nameof(Assignment.Label)),
                (Rule: IsInvalidX(assignment.Content), Parameter: nameof(Assignment.Content)),
                (Rule: IsInvalidX(assignment.Status), Parameter: nameof(Assignment.Status)),
                (Rule: IsInvalidX(assignment.Deadline), Parameter: nameof(Assignment.Deadline)),
                (Rule: IsInvalidX(assignment.CreatedBy), Parameter: nameof(Assignment.CreatedBy)),
                (Rule: IsInvalidX(assignment.UpdatedBy), Parameter: nameof(Assignment.UpdatedBy)),
                (Rule: IsInvalidX(assignment.CreatedDate), Parameter: nameof(Assignment.CreatedDate)),
                (Rule: IsInvalidX(assignment.UpdatedDate), Parameter: nameof(Assignment.UpdatedDate)),
                (Rule: IsNotRecent(assignment.CreatedDate), Parameter: nameof(Assignment.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: assignment.UpdatedBy,
                    secondId: assignment.CreatedBy,
                    secondIdName: nameof(Assignment.CreatedBy)),
                Parameter: nameof(Assignment.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: assignment.UpdatedDate,
                    secondDate: assignment.CreatedDate,
                    secondDateName: nameof(Assignment.CreatedDate)),
                Parameter: nameof(Assignment.UpdatedDate))
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

        private static dynamic IsInvalidX(AssignmentStatus status) => new
        {
            Condition = status != AssignmentStatus.Active,
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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

       private void ValidateAssignmentOnModify(Assignment assignment)
        {
            ValidateAssignmentIsNull(assignment);

            Validate(
                (Rule: IsInvalidX(assignment.Id), Parameter: nameof(Assignment.Id)),
                (Rule: IsInvalidX(assignment.Label), Parameter: nameof(Assignment.Label)),
                (Rule: IsInvalidX(assignment.Content), Parameter: nameof(Assignment.Content)),
                (Rule: IsInvalidX(assignment.Deadline), Parameter: nameof(Assignment.Deadline)),
                (Rule: IsInvalidX(assignment.CreatedBy), Parameter: nameof(Assignment.CreatedBy)),
                (Rule: IsInvalidX(assignment.UpdatedBy), Parameter: nameof(Assignment.UpdatedBy)),
                (Rule: IsInvalidX(assignment.CreatedDate), Parameter: nameof(Assignment.CreatedDate)),
                (Rule: IsInvalidX(assignment.UpdatedDate), Parameter: nameof(Assignment.UpdatedDate)),
                (Rule: IsNotRecent(assignment.UpdatedDate), Parameter: nameof(Assignment.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: assignment.UpdatedDate,
                    secondDate: assignment.CreatedDate,
                    secondDateName: nameof(Assignment.CreatedDate)),
                Parameter: nameof(Assignment.UpdatedDate))
            );
        }

        private static void ValidateAssignmentIsNull(Assignment assignment)
        {
            if (assignment is null)
            {
                throw new NullAssignmentException();
            }
        }

        private static void ValidateAssignmentIdIsNull(Guid assignmentId)
        {
            if (IsInvalid(assignmentId))
            {
                throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.Id),
                    parameterValue: assignmentId);
            }
        }

        private static void ValidateStorageAssignment(Assignment storageAssignment, Guid assignmentId)
        {
            if (storageAssignment == null)
            {
                throw new NotFoundAssignmentException(assignmentId);
            }
        }

        private static void ValidateAgainstStorageAssignmentOnModify(
            Assignment inputAssignment,
            Assignment storageAssignment)
        {
            switch (inputAssignment)
            {
                case { } when inputAssignment.CreatedDate != storageAssignment.CreatedDate:
                    throw new InvalidAssignmentException(
                        parameterName: nameof(Assignment.CreatedDate),
                        parameterValue: inputAssignment.CreatedDate);

                case { } when inputAssignment.CreatedBy != storageAssignment.CreatedBy:
                    throw new InvalidAssignmentException(
                        parameterName: nameof(Assignment.CreatedBy),
                        parameterValue: inputAssignment.CreatedBy);

                case { } when inputAssignment.UpdatedDate == storageAssignment.UpdatedDate:
                    throw new InvalidAssignmentException(
                        parameterName: nameof(Assignment.UpdatedDate),
                        parameterValue: inputAssignment.UpdatedDate);
            }
        }

        private static bool IsInvalid(Guid input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAssignmentException = new InvalidAssignmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAssignmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAssignmentException.ThrowIfContainsErrors();
        }
    }
}
