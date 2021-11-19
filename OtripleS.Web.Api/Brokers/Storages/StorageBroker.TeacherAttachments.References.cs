// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetTeacherAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherAttachment>()
                .HasKey(teacherAttachment =>
                    new { teacherAttachment.TeacherId, teacherAttachment.AttachmentId });

            modelBuilder.Entity<TeacherAttachment>()
                .HasOne(teacherAttachment => teacherAttachment.Attachment)
                .WithMany(attachment => attachment.TeacherAttachments)
                .HasForeignKey(teacherAttachment => teacherAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeacherAttachment>()
                .HasOne(teacherAttachment => teacherAttachment.Teacher)
                .WithMany(teacher => teacher.TeacherAttachments)
                .HasForeignKey(teacherAttachment => teacherAttachment.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
