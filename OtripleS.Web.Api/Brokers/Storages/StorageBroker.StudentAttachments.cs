// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentAttachment> StudentAttachments { get; set; }

        public async ValueTask<StudentAttachment> InsertStudentAttachmentAsync(
            StudentAttachment studentAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentAttachment> studentAttachmentEntityEntry =
                await broker.StudentAttachments.AddAsync(entity: studentAttachment);

            await broker.SaveChangesAsync();

            return studentAttachmentEntityEntry.Entity;
        }

        public IQueryable<StudentAttachment> SelectAllStudentAttachments() =>
            this.StudentAttachments;

        public async ValueTask<StudentAttachment> SelectStudentAttachmentByIdAsync(
            Guid studentId,
            Guid attachmentId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.StudentAttachments.FindAsync(studentId, attachmentId);
        }

        public async ValueTask<StudentAttachment> UpdateStudentAttachmentAsync(StudentAttachment StudentAttachment) =>
           await UpdateStudentAttachmentAsync(StudentAttachment);

        public async ValueTask<StudentAttachment> DeleteStudentAttachmentAsync(
            StudentAttachment studentAttachment)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentAttachment> studentAttachmentEntityEntry =
                broker.StudentAttachments.Remove(entity: studentAttachment);

            await broker.SaveChangesAsync();

            return studentAttachmentEntityEntry.Entity;
        }
    }
}
