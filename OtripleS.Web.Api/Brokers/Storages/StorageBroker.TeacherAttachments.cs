// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
                await broker.TeacherAttachments.AddAsync(entity: teacherAttachment);

            await broker.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }

        public IQueryable<TeacherAttachment> SelectAllTeacherAttachments() =>
            this.TeacherAttachments;

        public async ValueTask<TeacherAttachment> SelectTeacherAttachmentByIdAsync(Guid TeacherAttachmentId) =>
            await SelectTeacherAttachmentByIdAsync(TeacherAttachmentId);

        public async ValueTask<TeacherAttachment> UpdateTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                broker.TeacherAttachments.Update(entity: teacherAttachment);

            await broker.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }

        public async ValueTask<TeacherAttachment> DeleteTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherAttachment> teacherAttachmentEntityEntry =
                broker.TeacherAttachments.Remove(entity: teacherAttachment);

            await broker.SaveChangesAsync();

            return teacherAttachmentEntityEntry.Entity;
        }
    }
}
