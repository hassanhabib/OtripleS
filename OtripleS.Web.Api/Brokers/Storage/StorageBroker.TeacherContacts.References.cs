// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.TeacherContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddTeacherContactReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherContact>()
                .HasKey(teacherContact =>
                    new { teacherContact.TeacherId, teacherContact.ContactId });

            modelBuilder.Entity<TeacherContact>()
                .HasOne(teacherContact => teacherContact.Contact)
                .WithMany(contact => contact.TeacherContacts)
                .HasForeignKey(teacherContact => teacherContact.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeacherContact>()
                .HasOne(teacherContact => teacherContact.Teacher)
                .WithMany(teacher => teacher.TeacherContacts)
                .HasForeignKey(teacherContact => teacherContact.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
