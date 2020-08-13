// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentSemesterCourses;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddStudentSemesterCourseReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentSemesterCourse>()
                .HasKey(studentSemesterCourse =>
                    new { studentSemesterCourse.StudentId, studentSemesterCourse.SemesterCourseId });

            modelBuilder.Entity<StudentSemesterCourse>()
                .HasOne(studentSemesterCourse => studentSemesterCourse.SemesterCourse)
                .WithMany(semesterCourse => semesterCourse.StudentSemesterCourses)
                .HasForeignKey(studentSemesterCourse => studentSemesterCourse.SemesterCourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentSemesterCourse>()
                .HasOne(studentSemesterCourse => studentSemesterCourse.Student)
                .WithMany(student => student.StudentSemesterCourses)
                .HasForeignKey(studentSemesterCourse => studentSemesterCourse.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
