// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<CourseAttachment> CourseAttachments { get; set; }

        public async ValueTask<CourseAttachment> InsertCourseAttachmentAsync(
            CourseAttachment courseAttachment)
        {
            EntityEntry<CourseAttachment> courseAttachmentEntityEntry =
                await this.CourseAttachments.AddAsync(courseAttachment);

            await this.SaveChangesAsync();

            return courseAttachmentEntityEntry.Entity;
        }

        public IQueryable<CourseAttachment> SelectAllCourseAttachments() =>
            this.CourseAttachments.AsQueryable();

        public async ValueTask<CourseAttachment> SelectCourseAttachmentByIdAsync(
            Guid courseId,
            Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.CourseAttachments.FindAsync(courseId, attachmentId);
        }

        public async ValueTask<CourseAttachment> UpdateCourseAttachmentAsync(
            CourseAttachment courseAttachment)
        {
            EntityEntry<CourseAttachment> courseAttachmentEntityEntry =
                this.CourseAttachments.Update(courseAttachment);

            await this.SaveChangesAsync();

            return courseAttachmentEntityEntry.Entity;
        }

        public async ValueTask<CourseAttachment> DeleteCourseAttachmentAsync(
            CourseAttachment courseAttachment)
        {
            EntityEntry<CourseAttachment> courseAttachmentEntityEntry =
                this.CourseAttachments.Remove(courseAttachment);

            await this.SaveChangesAsync();

            return courseAttachmentEntityEntry.Entity;
        }
    }
}
