// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentExams;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<StudentExam> StudentExams { get; set; }

        public async ValueTask<StudentExam> InsertStudentExamAsync(StudentExam studentExam)
        {
            EntityEntry<StudentExam> studentExamEntityEntry = await this.StudentExams.AddAsync(studentExam);
            await this.SaveChangesAsync();

            return studentExamEntityEntry.Entity;
        }

        public IQueryable<StudentExam> SelectAllStudentExams() => this.StudentExams.AsQueryable();

        public async ValueTask<StudentExam> SelectStudentExamByIdAsync(Guid studentExamId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await StudentExams.FindAsync(studentExamId);
        }

        public async ValueTask<StudentExam> UpdateStudentExamAsync(StudentExam studentExam)
        {
            EntityEntry<StudentExam> studentExamEntityEntry = this.StudentExams.Update(studentExam);
            await this.SaveChangesAsync();

            return studentExamEntityEntry.Entity;
        }

        public async ValueTask<StudentExam> DeleteStudentExamAsync(StudentExam studentExam)
        {
            EntityEntry<StudentExam> studentExamEntityEntry = this.StudentExams.Remove(studentExam);
            await this.SaveChangesAsync();

            return studentExamEntityEntry.Entity;
        }
    }
}
