// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<CourseAttachment> CourseAttachments { get; set; }

        public async ValueTask<CourseAttachment> InsertCourseAttachmentAsync(
            CourseAttachment courseAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<CourseAttachment> courseAttachmentEntityEntry =
                await broker.CourseAttachments.AddAsync(entity: courseAttachment);

            await broker.SaveChangesAsync();

            return courseAttachmentEntityEntry.Entity;
        }

        public IQueryable<CourseAttachment> SelectAllCourseAttachments() =>
            this.CourseAttachments;

        public async ValueTask<CourseAttachment> SelectCourseAttachmentByIdAsync(
            Guid courseId,
            Guid attachmentId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.CourseAttachments.FindAsync(courseId, attachmentId);
        }

        public async ValueTask<CourseAttachment> UpdateCourseAttachmentAsync(
            CourseAttachment courseAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<CourseAttachment> courseAttachmentEntityEntry =
                broker.CourseAttachments.Update(entity: courseAttachment);

            await broker.SaveChangesAsync();

            return courseAttachmentEntityEntry.Entity;
        }

        public async ValueTask<CourseAttachment> DeleteCourseAttachmentAsync(
            CourseAttachment courseAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<CourseAttachment> courseAttachmentEntityEntry =
                broker.CourseAttachments.Remove(entity: courseAttachment);

            await broker.SaveChangesAsync();

            return courseAttachmentEntityEntry.Entity;
        }
    }
}
