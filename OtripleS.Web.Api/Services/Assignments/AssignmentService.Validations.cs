// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;

namespace OtripleS.Web.Api.Services.Assignments
{
    public partial class AssignmentService
    {
        private void ValidateAssignmentOnCreate(Assignment assignment)
        {
            ValidateAssignmentIsNull(assignment);
            ValidateAssignmentIdIsNull(assignment.Id);
            ValidateAssignmentFields(assignment);
            ValidateInvalidAuditFields(assignment);
            ValidateAuditFieldsDataOnCreate(assignment);
        }

        private void ValidateAssignmentOnModify(Assignment assignment)
        {
            ValidateAssignmentIsNull(assignment);
            ValidateAssignmentIdIsNull(assignment.Id);
            ValidateAssignmentFields(assignment);
            ValidateInvalidAuditFields(assignment);
            ValidateDatesAreNotSame(assignment);
            ValidateUpdatedDateIsRecent(assignment);
        }

        private void ValidateAssignmentIsNull(Assignment assignment)
        {
            if (assignment is null)
            {
                throw new NullAssignmentException();
            }
        }

        private void ValidateAssignmentIdIsNull(Guid assignmentId)
        {
            if (assignmentId == default)
            {
                throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.Id),
                    parameterValue: assignmentId);
            }
        }

        private void ValidateAssignmentFields(Assignment assignment)
        {
            if (IsInvalid(assignment.Label))
            {
                throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.Label),
                    parameterValue: assignment.Label);
            }

            if (IsInvalid(assignment.Content))
            {
                throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.Content),
                    parameterValue: assignment.Content);
            }
        }

        private void ValidateInvalidAuditFields(Assignment assignment)
        {
            switch (assignment)
            {
                case { } when IsInvalid(assignment.CreatedBy):
                    throw new InvalidAssignmentException(
                    parameterName: nameof(assignment.CreatedBy),
                    parameterValue: assignment.CreatedBy);

                case { } when IsInvalid(assignment.CreatedDate):
                    throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.CreatedDate),
                    parameterValue: assignment.CreatedDate);

                case { } when IsInvalid(assignment.UpdatedBy):
                    throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.UpdatedBy),
                    parameterValue: assignment.UpdatedBy);

                case { } when IsInvalid(assignment.UpdatedDate):
                    throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.UpdatedDate),
                    parameterValue: assignment.UpdatedDate);

                case { } when IsInvalid(assignment.Deadline):
                    throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.Deadline),
                    parameterValue: assignment.Deadline);
            }
        }

        private void ValidateAuditFieldsDataOnCreate(Assignment assignment)
        {
            switch (assignment)
            {
                case { } when assignment.UpdatedBy != assignment.CreatedBy:
                    throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.UpdatedBy),
                    parameterValue: assignment.UpdatedBy);

                case { } when assignment.UpdatedDate != assignment.CreatedDate:
                    throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.UpdatedDate),
                    parameterValue: assignment.UpdatedDate);

                case { } when IsDateNotRecent(assignment.CreatedDate):
                    throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.CreatedDate),
                    parameterValue: assignment.CreatedDate);
            }
        }

        private void ValidateDatesAreNotSame(Assignment assignment)
        {
            if (assignment.CreatedDate == assignment.UpdatedDate)
            {
                throw new InvalidAssignmentException(
                    parameterName: nameof(Assignment.UpdatedDate),
                    parameterValue: assignment.UpdatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Assignment assignment)
        {
            if (IsDateNotRecent(assignment.UpdatedDate))
            {
                throw new InvalidAssignmentException(
                    parameterName: nameof(assignment.UpdatedDate),
                    parameterValue: assignment.UpdatedDate);
            }
        }

        private void ValidateStorageAssignment(Assignment storageAssignment, Guid assignmentId)
        {
            if (storageAssignment == null)
            {
                throw new NotFoundAssignmentException(assignmentId);
            }
        }

        private void ValidateAgainstStorageAssignmentOnModify(Assignment inputAssignment, Assignment storageAssignment)
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

        private void ValidateStorageAssignments(IQueryable<Assignment> storageAssignments)
        {
            if (storageAssignments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No Assignments found in storage.");
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
