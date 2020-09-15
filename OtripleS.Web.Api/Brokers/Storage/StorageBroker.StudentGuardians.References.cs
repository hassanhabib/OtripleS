// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddStudentGuardianReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentGuardian>()
                .HasKey(studentGuardian =>
                    new { studentGuardian.StudentId, studentGuardian.GuardianId });

            modelBuilder.Entity<StudentGuardian>()
                .HasOne(studentGuardian => studentGuardian.Guardian)
                .WithMany(guardian => guardian.StudentGuardians)
                .HasForeignKey(studentGuardian => studentGuardian.GuardianId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentGuardian>()
                .HasOne(studentGuardian => studentGuardian.Student)
                .WithMany(student => student.StudentGuardians)
                .HasForeignKey(studentGuardian => studentGuardian.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
