// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.AssignmentAttachments;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddAssignmentAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssignmentAttachment>()
                .HasKey(assignmentAttachment =>
                    new { assignmentAttachment.AssignmentId, assignmentAttachment.AttachmentId });

            modelBuilder.Entity<AssignmentAttachment>()
                .HasOne(assignmentAttachment => assignmentAttachment.Assignment)
                .WithMany(assignment => assignment.AssignmentAttachments)
                .HasForeignKey(assignmentAttachment => assignmentAttachment.AssignmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AssignmentAttachment>()
                .HasOne(assignmentAttachment => assignmentAttachment.Attachment)
                .WithMany(attachment => attachment.AssignmentAttachments)
                .HasForeignKey(assignmentAttachment => assignmentAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }    
}
