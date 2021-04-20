// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddStudentExamFeeReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentExamFee>()
                .HasOne(studentExamFee => studentExamFee.CreatedByUser)
                .WithMany(studentExamFeeCreatedByUser => studentExamFeeCreatedByUser.StudentExamFeesCreatedByUser)
                .HasForeignKey(examfee => examfee.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentExamFee>()
                .HasOne(studentExamFee => studentExamFee.UpdatedByUser)
                .WithMany(studentExamFeeUpdatedByUser => studentExamFeeUpdatedByUser.StudentExamFeesUpdatedByUser)
                .HasForeignKey(examfee => examfee.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentExamFee>()
                .HasOne(studentExamFee => studentExamFee.ExamFee)
                .WithMany(studentExamFee => studentExamFee.StudentExamFees)
                .HasForeignKey(studentExamFee => studentExamFee.ExamFeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentExamFee>()
                .HasOne(studentExamFee => studentExamFee.Student)
                .WithMany(studentExamFee => studentExamFee.StudentExamFees)
                .HasForeignKey(studentExamFee => studentExamFee.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
