﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.GuardianAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GuardianAttachment> GuardianAttachments { get; set; }

        public async ValueTask<GuardianAttachment> InsertGuardianAttachmentAsync(
            GuardianAttachment guardianAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<GuardianAttachment> guardianAttachmentEntityEntry =
                await broker.GuardianAttachments.AddAsync(entity: guardianAttachment);

            await broker.SaveChangesAsync();

            return guardianAttachmentEntityEntry.Entity;
        }

        public IQueryable<GuardianAttachment> SelectAllGuardianAttachments() =>
            this.GuardianAttachments;

        public async ValueTask<GuardianAttachment> SelectGuardianAttachmentByIdAsync(
            Guid guardianId,
            Guid attachmentId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.GuardianAttachments.FindAsync(guardianId, attachmentId);
        }

        public async ValueTask<GuardianAttachment> UpdateGuardianAttachmentAsync(
            GuardianAttachment guardianAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<GuardianAttachment> guardianAttachmentEntityEntry =
                broker.GuardianAttachments.Update(entity: guardianAttachment);

            await broker.SaveChangesAsync();

            return guardianAttachmentEntityEntry.Entity;
        }

        public async ValueTask<GuardianAttachment> DeleteGuardianAttachmentAsync(
            GuardianAttachment guardianAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<GuardianAttachment> guardianAttachmentEntityEntry =
                broker.GuardianAttachments.Remove(entity: guardianAttachment);

            await broker.SaveChangesAsync();

            return guardianAttachmentEntityEntry.Entity;
        }
    }
}
