// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentExams;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        private void AddStudentExamReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentExam>()
                .HasOne(studentExam => studentExam.Exam)
                .WithMany(exam => exam.StudentExams)
                .HasForeignKey(studentExam => studentExam.ExamId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentExam>()
                .HasOne(studentExam => studentExam.ReviewingTeacher)
                .WithMany(teacher => teacher.ReviewedStudentExams)
                .HasForeignKey(studentExam => studentExam.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentExam>()
                .HasOne(studentExam => studentExam.Student)
                .WithMany(student => student.StudentExams)
                .HasForeignKey(studentExam => studentExam.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
