// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddStudentRegistrationReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRegistration>()
                .HasKey(studentRegistration =>
                    new { studentRegistration.StudentId, studentRegistration.RegistrationId });

            modelBuilder.Entity<StudentRegistration>()
                .HasOne(studentregistration => studentregistration.Student)
                .WithMany(studentregistration => studentregistration.StudentRegistrations)
                .HasForeignKey(studentregistration => studentregistration.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentRegistration>()
                .HasOne(studentregistration => studentregistration.Registration)
                .WithMany(studentregistration => studentregistration.StudentRegistrations)
                .HasForeignKey(studentregistration => studentregistration.RegistrationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
