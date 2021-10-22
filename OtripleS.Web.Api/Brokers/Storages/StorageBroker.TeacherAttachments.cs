// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<TeacherAttachment> TeacherAttachments { get; set; }

        public async ValueTask<TeacherAttachment> InsertTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                await broker.TeacherAttachments.AddAsync(teacherAttachment);

            await broker.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }

        public IQueryable<TeacherAttachment> SelectAllTeacherAttachments() =>
            this.TeacherAttachments.AsQueryable();

        public async ValueTask<TeacherAttachment> SelectTeacherAttachmentByIdAsync(
            Guid teacherId,
            Guid attachmentId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.TeacherAttachments.FindAsync(teacherId, attachmentId);
        }

        public async ValueTask<TeacherAttachment> UpdateTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                broker.TeacherAttachments.Update(teacherAttachment);

            await broker.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }

        public async ValueTask<TeacherAttachment> DeleteTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                broker.TeacherAttachments.Remove(teacherAttachment);

            await broker.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }
    }
}
