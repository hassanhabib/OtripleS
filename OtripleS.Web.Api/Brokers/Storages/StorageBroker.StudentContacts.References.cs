// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Foundations.StudentContacts;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddStudentContactReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentContact>()
                .HasKey(studentContact =>
                    new { studentContact.StudentId, studentContact.ContactId });

            modelBuilder.Entity<StudentContact>()
                .HasOne(studentContact => studentContact.Contact)
                .WithMany(contact => contact.StudentContacts)
                .HasForeignKey(studentContact => studentContact.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentContact>()
                .HasOne(studentContact => studentContact.Student)
                .WithMany(student => student.StudentContacts)
                .HasForeignKey(studentContact => studentContact.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
