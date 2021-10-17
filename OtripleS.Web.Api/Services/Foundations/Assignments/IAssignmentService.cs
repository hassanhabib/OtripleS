// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Assignments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.Assignments
{
    public interface IAssignmentService
    {
        ValueTask<Assignment> CreateAssignmentAsync(Assignment assignemnt);
        IQueryable<Assignment> RetrieveAllAssignments();
        ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid guid);
        ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment);
        ValueTask<Assignment> RemoveAssignmentByIdAsync(Guid assignmentId);
    }
}
