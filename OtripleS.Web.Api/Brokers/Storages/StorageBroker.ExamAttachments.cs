﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
            var broker = new StorageBroker(this.configuration);

            EntityEntry<ExamAttachment> examAttachmentEntityEntry =
                await broker.ExamAttachments.AddAsync(entity: examAttachment);

            await broker.SaveChangesAsync();

            return examAttachmentEntityEntry.Entity;
        }

        public IQueryable<ExamAttachment> SelectAllExamAttachments() =>
            this.ExamAttachments;

        public async ValueTask<ExamAttachment> SelectExamAttachmentByIdAsync(
            Guid examId,
            Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.ExamAttachments.FindAsync(examId, attachmentId);
        }

        public async ValueTask<ExamAttachment> UpdateExamAttachmentAsync(ExamAttachment ExamAttachment) =>
             await UpdateExamAttachmentAsync(ExamAttachment);

        public async ValueTask<ExamAttachment> DeleteExamAttachmentAsync(
            ExamAttachment examAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<ExamAttachment> examAttachmentEntityEntry =
                broker.ExamAttachments.Remove(entity: examAttachment);

            await broker.SaveChangesAsync();

            return examAttachmentEntityEntry.Entity;
        }
    }
}
