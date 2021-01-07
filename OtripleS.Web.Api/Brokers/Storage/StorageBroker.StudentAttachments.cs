// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentAttachments;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<StudentAttachment> StudentAttachments { get; set; }

        public async ValueTask<StudentAttachment> InsertStudentAttachmentAsync(
            StudentAttachment StudentAttachment)
        {
            EntityEntry<StudentAttachment> StudentAttachmentEntityEntry =
                await this.StudentAttachments.AddAsync(StudentAttachment);

            await this.SaveChangesAsync();

            return StudentAttachmentEntityEntry.Entity;
        }

        public IQueryable<StudentAttachment> SelectAllStudentAttachments() =>
            this.StudentAttachments.AsQueryable();

        public async ValueTask<StudentAttachment> SelectStudentAttachmentByIdAsync(
            Guid studentId,
            Guid attachmentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.StudentAttachments.FindAsync(studentId, attachmentId);
        }

        public async ValueTask<StudentAttachment> UpdateStudentAttachmentAsync(
            StudentAttachment StudentAttachment)
        {
            EntityEntry<StudentAttachment> StudentAttachmentEntityEntry =
                this.StudentAttachments.Update(StudentAttachment);

            await this.SaveChangesAsync();

            return StudentAttachmentEntityEntry.Entity;
        }

        public async ValueTask<StudentAttachment> DeleteStudentAttachmentAsync(
            StudentAttachment StudentAttachment)
        {
            EntityEntry<StudentAttachment> StudentAttachmentEntityEntry =
                this.StudentAttachments.Remove(StudentAttachment);

            await this.SaveChangesAsync();

            return StudentAttachmentEntityEntry.Entity;
        }
    }
}
