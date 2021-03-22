// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddFeeReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fee>()
                .HasOne(fee => fee.CreatedByUser)
                .WithMany(feeCreatedByUser => feeCreatedByUser.Fees)
                .HasForeignKey(fee => fee.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
