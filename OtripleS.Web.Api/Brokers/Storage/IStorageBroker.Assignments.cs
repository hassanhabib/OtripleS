// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Assignments;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Assignment> InsertAssignmentAsync(Assignment Classroom);
        IQueryable<Assignment> SelectAllAssignments();
        ValueTask<Assignment> SelectAssignmentByIdAsync(Guid ClassroomId);
        ValueTask<Assignment> UpdateAssignmentAsync(Assignment Classroom);
        ValueTask<Assignment> DeleteAssignmentAsync(Assignment Classroom);
    }
}
