// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Foundations.AssignmentAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<AssignmentAttachment> AssignmentAttachments { get; set; }

        public async ValueTask<AssignmentAttachment> InsertAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            EntityEntry<AssignmentAttachment> assignmentAttachmentEntityEntry =
                await this.AssignmentAttachments.AddAsync(assignmentAttachment);

            await this.SaveChangesAsync();

            return assignmentAttachmentEntityEntry.Entity;
        }

        public IQueryable<AssignmentAttachment> SelectAllAssignmentAttachments() =>
            this.AssignmentAttachments.AsQueryable();

        public async ValueTask<AssignmentAttachment> SelectAssignmentAttachmentByIdAsync(
            Guid assignmentId,
            Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.AssignmentAttachments.FindAsync(assignmentId, attachmentId);
        }

        public async ValueTask<AssignmentAttachment> UpdateAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            EntityEntry<AssignmentAttachment> assignmentAttachmentEntityEntry =
                this.AssignmentAttachments.Update(assignmentAttachment);

            await this.SaveChangesAsync();

            return assignmentAttachmentEntityEntry.Entity;
        }

        public async ValueTask<AssignmentAttachment> DeleteAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            EntityEntry<AssignmentAttachment> assignmentAttachmentEntityEntry =
                this.AssignmentAttachments.Remove(assignmentAttachment);

            await this.SaveChangesAsync();

            return assignmentAttachmentEntityEntry.Entity;
        }
    }
}
