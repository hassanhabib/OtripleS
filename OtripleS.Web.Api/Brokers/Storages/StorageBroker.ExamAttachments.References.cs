// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.ExamAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetExamAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamAttachment>()
                .HasKey(examAttachment =>
                    new { examAttachment.ExamId, examAttachment.AttachmentId });

            modelBuilder.Entity<ExamAttachment>()
                .HasOne(examAttachment => examAttachment.Attachment)
                .WithMany(attachment => attachment.ExamAttachments)
                .HasForeignKey(examAttachment => examAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamAttachment>()
                .HasOne(examAttachment => examAttachment.Exam)
                .WithMany(exam => exam.ExamAttachments)
                .HasForeignKey(examAttachment => examAttachment.ExamId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
