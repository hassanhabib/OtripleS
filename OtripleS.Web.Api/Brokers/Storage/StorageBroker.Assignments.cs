// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Assignments;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Assignment> Assignments { get; set; }

        public async ValueTask<Assignment> InsertAssignmentAsync(Assignment assignment)
        {
            EntityEntry<Assignment> assignmentEntityEntry = await this.Assignments.AddAsync(assignment);
            await this.SaveChangesAsync();

            return assignmentEntityEntry.Entity;
        }

        public IQueryable<Assignment> SelectAllAssignments() => this.Assignments.AsQueryable();

        public async ValueTask<Assignment> SelectAssignmentByIdAsync(Guid assignmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Assignments.FindAsync(assignmentId);
        }

        public async ValueTask<Assignment> UpdateAssignmentAsync(Assignment assignment)
        {
            EntityEntry<Assignment> assignmentEntityEntry = this.Assignments.Update(assignment);
            await this.SaveChangesAsync();

            return assignmentEntityEntry.Entity;
        }

        public async ValueTask<Assignment> DeleteAssignmentAsync(Assignment assignment)
        {
            EntityEntry<Assignment> assignmentEntityEntry = this.Assignments.Remove(assignment);
            await this.SaveChangesAsync();

            return assignmentEntityEntry.Entity;
        }
    }
}
