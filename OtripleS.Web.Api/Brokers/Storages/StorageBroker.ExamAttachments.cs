// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.ExamAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ExamAttachment> ExamAttachments { get; set; }

        public async ValueTask<ExamAttachment> InsertExamAttachmentAsync(
            ExamAttachment examAttachment)
        {
            EntityEntry<ExamAttachment> examAttachmentEntityEntry =
                await this.ExamAttachments.AddAsync(examAttachment);

            await this.SaveChangesAsync();

            return examAttachmentEntityEntry.Entity;
        }

        public IQueryable<ExamAttachment> SelectAllExamAttachments() =>
            this.ExamAttachments.AsQueryable();

        public async ValueTask<ExamAttachment> SelectExamAttachmentByIdAsync(
            Guid examId,
            Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.ExamAttachments.FindAsync(examId, attachmentId);
        }

        public async ValueTask<ExamAttachment> UpdateExamAttachmentAsync(
            ExamAttachment examAttachment)
        {
            EntityEntry<ExamAttachment> examAttachmentEntityEntry =
                this.ExamAttachments.Update(examAttachment);

            await this.SaveChangesAsync();

            return examAttachmentEntityEntry.Entity;
        }

        public async ValueTask<ExamAttachment> DeleteExamAttachmentAsync(
            ExamAttachment examAttachment)
        {
            EntityEntry<ExamAttachment> examAttachmentEntityEntry =
                this.ExamAttachments.Remove(examAttachment);

            await this.SaveChangesAsync();

            return examAttachmentEntityEntry.Entity;
        }
    }
}
