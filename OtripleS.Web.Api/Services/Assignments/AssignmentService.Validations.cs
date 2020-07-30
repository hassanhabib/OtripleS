// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using OtripleS.Web.Api.Models.Assignments;

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
    }
}