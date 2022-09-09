// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.AssignmentAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<AssignmentAttachment> AssignmentAttachments { get; set; }

        public async ValueTask<AssignmentAttachment> InsertAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<AssignmentAttachment> assignmentAttachmentEntityEntry =
                await broker.AssignmentAttachments.AddAsync(entity: assignmentAttachment);

            await broker.SaveChangesAsync();

            return assignmentAttachmentEntityEntry.Entity;
        }

        public IQueryable<AssignmentAttachment> SelectAllAssignmentAttachments() =>
            this.AssignmentAttachments;

        public async ValueTask<AssignmentAttachment> SelectAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.AssignmentAttachments.FindAsync(assignmentId, attachmentId);
        }

        public async ValueTask<AssignmentAttachment> UpdateAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<AssignmentAttachment> assignmentAttachmentEntityEntry =
                broker.AssignmentAttachments.Update(entity: assignmentAttachment);

            await broker.SaveChangesAsync();

            return assignmentAttachmentEntityEntry.Entity;
        }

        public async ValueTask<AssignmentAttachment> DeleteAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<AssignmentAttachment> assignmentAttachmentEntityEntry =
                broker.AssignmentAttachments.Remove(entity: assignmentAttachment);

            await broker.SaveChangesAsync();

            return assignmentAttachmentEntityEntry.Entity;
        }
    }
}
