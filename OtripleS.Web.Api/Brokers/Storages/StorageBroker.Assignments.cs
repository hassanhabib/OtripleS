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

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Assignment> Assignments { get; set; }

        public async ValueTask<Assignment> InsertAssignmentAsync(Assignment assignment)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Assignment> assignmentEntityEntry = await broker.Assignments.AddAsync(entity: assignment);
            await broker.SaveChangesAsync();

            return assignmentEntityEntry.Entity;
        }

        public IQueryable<Assignment> SelectAllAssignments() => this.Assignments;

        public async ValueTask<Assignment> SelectAssignmentByIdAsync(Guid assignmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Assignments.FindAsync(assignmentId);
        }

        public async ValueTask<Assignment> UpdateAssignmentAsync(Assignment assignment)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Assignment> assignmentEntityEntry = broker.Assignments.Update(entity: assignment);
            await broker.SaveChangesAsync();

            return assignmentEntityEntry.Entity;
        }

        public async ValueTask<Assignment> DeleteAssignmentAsync(Assignment assignment)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Assignment> assignmentEntityEntry = broker.Assignments.Remove(entity: assignment);
            await broker.SaveChangesAsync();

            return assignmentEntityEntry.Entity;
        }
    }
}
