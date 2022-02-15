// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.GuardianAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetGuardianAttachmentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuardianAttachment>()
                .HasKey(guardianAttachment =>
                    new { guardianAttachment.GuardianId, guardianAttachment.AttachmentId });

            modelBuilder.Entity<GuardianAttachment>()
                .HasOne(guardianAttachment => guardianAttachment.Attachment)
                .WithMany(attachment => attachment.GuardianAttachments)
                .HasForeignKey(guardianAttachment => guardianAttachment.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GuardianAttachment>()
                .HasOne(guardianAttachment => guardianAttachment.Guardian)
                .WithMany(guardian => guardian.GuardianAttachments)
                .HasForeignKey(guardianAttachment => guardianAttachment.GuardianId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
