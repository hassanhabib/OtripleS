// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Attachments;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Attachment> Attachments { get; set; }
        public async ValueTask<Attachment> InsertAttachmentAsync(Attachment attachment)
        {
            EntityEntry<Attachment> attachmentEntityEntry = await this.Attachments.AddAsync(attachment);
            await this.SaveChangesAsync();

            return attachmentEntityEntry.Entity;
        }

        public IQueryable<Attachment> SelectAllAttachments() => this.Attachments.AsQueryable();

        public async ValueTask<Attachment> SelectAttachmentByIdAsync(Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Attachments.FindAsync(attachmentId);
        }

        public async ValueTask<Attachment> UpdateAttachmentAsync(Attachment attachment)
        {
            EntityEntry<Attachment> attachmentEntityEntry = this.Attachments.Update(attachment);
            await this.SaveChangesAsync();

            return attachmentEntityEntry.Entity;
        }

        public async ValueTask<Attachment> DeleteAttachmentAsync(Attachment attachment)
        {
            EntityEntry<Attachment> attachmentEntityEntry = this.Attachments.Remove(attachment);
            await this.SaveChangesAsync();

            return attachmentEntityEntry.Entity;
        }
    }
}
