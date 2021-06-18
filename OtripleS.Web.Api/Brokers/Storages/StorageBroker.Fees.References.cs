// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Foundations.Fees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddFeeReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fee>()
                .HasOne(fee => fee.CreatedByUser)
                .WithMany(feeCreatedByUser => feeCreatedByUser.FeesCreatedByUser)
                .HasForeignKey(fee => fee.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Fee>()
                .HasOne(fee => fee.UpdatedByUser)
                .WithMany(feeUpdatedByUser => feeUpdatedByUser.FeesUpdatedByUser)
                .HasForeignKey(fee => fee.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
