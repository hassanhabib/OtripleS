// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentExams;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentExam> StudentExams { get; set; }

        public async ValueTask<StudentExam> InsertStudentExamAsync(StudentExam studentExam)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentExam> studentExamEntityEntry =
                await broker.StudentExams.AddAsync(entity: studentExam);

            await broker.SaveChangesAsync();

            return studentExamEntityEntry.Entity;
        }

        public IQueryable<StudentExam> SelectAllStudentExams() => this.StudentExams;

        public async ValueTask<StudentExam> SelectStudentExamByIdAsync(Guid studentExamId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.StudentExams.FindAsync(studentExamId);
        }

        public async ValueTask<StudentExam> UpdateStudentExamAsync(StudentExam studentExam)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<StudentExam> studentExamEntityEntry = broker.StudentExams.Update(entity: studentExam);
            await broker.SaveChangesAsync();

            return studentExamEntityEntry.Entity;
        }

        public async ValueTask<StudentExam> DeleteStudentExamAsync(StudentExam studentExam)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<StudentExam> studentExamEntityEntry = broker.StudentExams.Remove(entity: studentExam);
            await broker.SaveChangesAsync();

            return studentExamEntityEntry.Entity;
        }
    }
}
