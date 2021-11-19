// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.ExamFees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetExamFeeReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamFee>()
                .HasOne(examFee => examFee.CreatedByUser)
                .WithMany(examFeeCreatedByUser => examFeeCreatedByUser.ExamFeesCreatedByUser)
                .HasForeignKey(examfee => examfee.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamFee>()
                .HasOne(examFee => examFee.UpdatedByUser)
                .WithMany(examFeeUpdatedByUser => examFeeUpdatedByUser.ExamFeesUpdatedByUser)
                .HasForeignKey(examfee => examfee.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamFee>()
                .HasOne(examFee => examFee.Exam)
                .WithMany(examFee => examFee.ExamFees)
                .HasForeignKey(examfee => examfee.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamFee>()
                .HasOne(examFee => examFee.Fee)
                .WithMany(examFee => examFee.ExamFees)
                .HasForeignKey(examfee => examfee.FeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
