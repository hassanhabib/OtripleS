// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Registrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private void AddRegistrationReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>()
                .HasOne(registration => registration.CreatedByUser)
                .WithMany(registrationCreatedByUser => registrationCreatedByUser.RegistrationsCreatedByUser)
                .HasForeignKey(registration => registration.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Registration>()
                .HasOne(registration => registration.UpdatedByUser)
                .WithMany(registrationUpdatedByUser => registrationUpdatedByUser.RegistrationsUpdatedByUser)
                .HasForeignKey(registration => registration.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
