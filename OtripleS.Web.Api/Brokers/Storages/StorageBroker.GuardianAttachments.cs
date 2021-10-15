// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.GuardianAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GuardianAttachment> GuardianAttachments { get; set; }

        public async ValueTask<GuardianAttachment> InsertGuardianAttachmentAsync(
            GuardianAttachment guradianAttachment)
        {
            EntityEntry<GuardianAttachment> guradianAttachmentEntityEntry =
                await this.GuardianAttachments.AddAsync(guradianAttachment);

            await this.SaveChangesAsync();

            return guradianAttachmentEntityEntry.Entity;
        }

        public IQueryable<GuardianAttachment> SelectAllGuardianAttachments() =>
            this.GuardianAttachments.AsQueryable();

        public async ValueTask<GuardianAttachment> SelectGuardianAttachmentByIdAsync(
            Guid guradianId,
            Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.GuardianAttachments.FindAsync(guradianId, attachmentId);
        }

        public async ValueTask<GuardianAttachment> UpdateGuardianAttachmentAsync(
            GuardianAttachment guradianAttachment)
        {
            EntityEntry<GuardianAttachment> guradianAttachmentEntityEntry =
                this.GuardianAttachments.Update(guradianAttachment);

            await this.SaveChangesAsync();

            return guradianAttachmentEntityEntry.Entity;
        }

        public async ValueTask<GuardianAttachment> DeleteGuardianAttachmentAsync(
            GuardianAttachment guradianAttachment)
        {
            EntityEntry<GuardianAttachment> guradianAttachmentEntityEntry =
                this.GuardianAttachments.Remove(guradianAttachment);

            await this.SaveChangesAsync();

            return guradianAttachmentEntityEntry.Entity;
        }
    }
}
