﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Assignments;

namespace OtripleS.Web.Api.Services.Foundations.Assignments
{
    public interface IAssignmentService
    {
        ValueTask<Assignment> CreateAssignmentAsync(Assignment assignment);
        IQueryable<Assignment> RetrieveAllAssignments();
        ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid guid);
        ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment);
        ValueTask<Assignment> RemoveAssignmentByIdAsync(Guid assignmentId);
    }
}
