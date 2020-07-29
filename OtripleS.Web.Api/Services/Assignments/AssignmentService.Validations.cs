// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;

        private void ValidateInvalidAuditFields(Assignment assignment)
        {
            switch (assignment)
            {
                case { } when IsInvalid(assignment.CreatedBy):
                    throw new InvalidAssignmentException(
                    parameterName: nameof(assignment.CreatedBy),
                    parameterValue: assignment.CreatedBy);
            }
        }
    }
}
