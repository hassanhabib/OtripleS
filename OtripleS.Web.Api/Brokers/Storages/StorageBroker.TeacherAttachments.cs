// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Foundations.TeacherAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<TeacherAttachment> TeacherAttachments { get; set; }

        public async ValueTask<TeacherAttachment> InsertTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                await this.TeacherAttachments.AddAsync(teacherAttachment);

            await this.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }

        public IQueryable<TeacherAttachment> SelectAllTeacherAttachments() =>
            this.TeacherAttachments.AsQueryable();

        public async ValueTask<TeacherAttachment> SelectTeacherAttachmentByIdAsync(
            Guid teacherId,
            Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.TeacherAttachments.FindAsync(teacherId, attachmentId);
        }

        public async ValueTask<TeacherAttachment> UpdateTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                this.TeacherAttachments.Update(teacherAttachment);

            await this.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }

        public async ValueTask<TeacherAttachment> DeleteTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                this.TeacherAttachments.Remove(teacherAttachment);

            await this.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }
    }
}
