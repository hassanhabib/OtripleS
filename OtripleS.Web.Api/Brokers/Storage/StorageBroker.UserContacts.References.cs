// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddUserContactReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserContact>()
                .HasKey(userContact =>
                    new { userContact.UserId, userContact.ContactId });

            modelBuilder.Entity<UserContact>()
                .HasOne(userContact => userContact.Contact)
                .WithMany(contact => contact.UserContacts)
                .HasForeignKey(userContact => userContact.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserContact>()
                .HasOne(userContact => userContact.User)
                .WithMany(User => User.UserContacts)
                .HasForeignKey(userContact => userContact.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
