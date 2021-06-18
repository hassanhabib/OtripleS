// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Foundations.CourseAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddCourseAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseAttachment>()
                .HasKey(courseAttachment =>
                    new { courseAttachment.CourseId, courseAttachment.AttachmentId });

            modelBuilder.Entity<CourseAttachment>()
                .HasOne(courseAttachment => courseAttachment.Attachment)
                .WithMany(attachment => attachment.CourseAttachments)
                .HasForeignKey(courseAttachment => courseAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseAttachment>()
                .HasOne(courseAttachment => courseAttachment.Course)
                .WithMany(course => course.CourseAttachments)
                .HasForeignKey(courseAttachment => courseAttachment.CourseId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
