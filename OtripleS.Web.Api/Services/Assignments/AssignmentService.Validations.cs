// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.AspNetCore.Server.IIS.Core;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;

namespace OtripleS.Web.Api.Services.Assignments
{
    public partial class AssignmentService
    {
        private void ValidateStorageAssignments(IQueryable<Assignment> storageAssignments)
        {
            if (storageAssignments.Count() == 0)
            {
                this.loggingBroker.LogWarning("No Assignments found in storage.");
            }
        }

        private void ValidateStorageAssignment(Assignment storageAssignment)
        {
            if (storageAssignment == null)
            {
                this.loggingBroker.LogWarning("No Assignment found in storage.");
            }
        }

        private void ValidateStorageAssignment(Assignment storageAssignment, Guid guid)
        {
            ValidateStorageAssignment(storageAssignment);
            if (!guid.Equals(storageAssignment.Id))
            {
                throw new InvalidAssignmentException(nameof(Assignment.Id), guid);
            }
        }

        private void ValidateStorageIdIsNotNullOrEmpty(Guid guid)
        {
            if (guid == default)
            {
                throw new InvalidAssignmentException(nameof(Assignment.Id), guid);
            }
        }
    }
}