// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddExamReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exam>()
                .HasOne(exam => exam.SemesterCourse)
                .WithMany(semesterCourse => semesterCourse.Exams)
                .HasForeignKey(exam => exam.SemesterCourseId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
