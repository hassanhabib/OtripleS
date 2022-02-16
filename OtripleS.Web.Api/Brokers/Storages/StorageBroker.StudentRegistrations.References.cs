// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetStudentRegistrationReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRegistration>()
                .HasKey(studentRegistration =>
                    new { studentRegistration.StudentId, studentRegistration.RegistrationId });

            modelBuilder.Entity<StudentRegistration>()
                .HasOne(studentRegistration => studentRegistration.Student)
                .WithMany(studentRegistration => studentRegistration.StudentRegistrations)
                .HasForeignKey(studentRegistration => studentRegistration.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentRegistration>()
                .HasOne(studentRegistration => studentRegistration.Registration)
                .WithMany(studentRegistration => studentRegistration.StudentRegistrations)
                .HasForeignKey(studentRegistration => studentRegistration.RegistrationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
