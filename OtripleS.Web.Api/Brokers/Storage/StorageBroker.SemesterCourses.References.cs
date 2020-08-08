// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.SemesterCourses;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddSemesterCourseReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SemesterCourse>()
                .HasOne(semesterCourse => semesterCourse.Teacher)
                .WithMany(teacher => teacher.SemesterCourses)
                .HasForeignKey(semesterCourse => semesterCourse.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SemesterCourse>()
                .HasOne(semesterCourse => semesterCourse.Course)
                .WithMany(course => course.SemesterCourses)
                .HasForeignKey(semesterCourse => semesterCourse.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SemesterCourse>()
                .HasOne(semesterCourse => semesterCourse.Classroom)
                .WithMany(classroom => classroom.SemesterCourses)
                .HasForeignKey(semesterCourse => semesterCourse.ClassroomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
