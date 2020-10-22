// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.GuardianContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddGuardianContactReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuardianContact>()
                .HasKey(guardianContact =>
                    new { guardianContact.GuardianId, guardianContact.ContactId });

            modelBuilder.Entity<GuardianContact>()
                .HasOne(guardianContact => guardianContact.Contact)
                .WithMany(contact => contact.GuardianContacts)
                .HasForeignKey(guardianContact => guardianContact.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GuardianContact>()
                .HasOne(guardianContact => guardianContact.Guardian)
                .WithMany(guardian => guardian.GuardianContacts)
                .HasForeignKey(guardianContact => guardianContact.GuardianId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
