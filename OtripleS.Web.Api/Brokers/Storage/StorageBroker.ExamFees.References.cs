// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.ExamFees;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddExamFeeReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamFee>()
                .HasOne(examFee => examFee.CreatedByUser)
                .WithMany(examFeeCreatedByUser => examFeeCreatedByUser.ExamFeesUpdatedByUser)
                .HasForeignKey(examfee => examfee.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamFee>()
                .HasOne(examFee => examFee.UpdatedByUser)
                .WithMany(examFeeUpdatedByUser => examFeeUpdatedByUser.ExamFeesUpdatedByUser)
                .HasForeignKey(examfee => examfee.FeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
