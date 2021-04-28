// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private void AddStudentAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentAttachment>()
                .HasKey(studentAttachment =>
                    new { studentAttachment.StudentId, studentAttachment.AttachmentId });

            modelBuilder.Entity<StudentAttachment>()
                .HasOne(studentAttachment => studentAttachment.Attachment)
                .WithMany(attachment => attachment.StudentAttachments)
                .HasForeignKey(studentAttachment => studentAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentAttachment>()
                .HasOne(studentAttachment => studentAttachment.Student)
                .WithMany(student => student.StudentAttachments)
                .HasForeignKey(studentAttachment => studentAttachment.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
