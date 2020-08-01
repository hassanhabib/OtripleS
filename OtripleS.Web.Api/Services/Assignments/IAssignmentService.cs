// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Assignments;

namespace OtripleS.Web.Api.Services.Assignments
{
    public interface IAssignmentService
    {
        ValueTask<Assignment> CreateAssignmentAsync(Assignment assignemnt);
        ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment);
        IQueryable<Assignment> RetrieveAllAssignments();
        ValueTask<Assignment> RetrieveAssignmentById(Guid guid);
        ValueTask<Assignment> DeleteAssignmentAsync(Guid assignmentId);
    }
}
