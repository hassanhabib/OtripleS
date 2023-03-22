// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Exam> Exams { get; set; }

        public async ValueTask<Exam> InsertExamAsync(Exam exam)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Exam> examEntityEntry = await broker.Exams.AddAsync(entity: exam);
            await broker.SaveChangesAsync();

            return examEntityEntry.Entity;
        }

        public IQueryable<Exam> SelectAllExams() => this.Exams;

        public async ValueTask<Exam> SelectExamByIdAsync(Guid examId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Exams.FindAsync(examId);
        }

        public async ValueTask<Exam> UpdateExamAsync(Exam Exam) =>
          await UpdateExamAsync(Exam);

        public async ValueTask<Exam> DeleteExamAsync(Exam exam)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Exam> examEntityEntry = broker.Exams.Remove(entity: exam);
            await broker.SaveChangesAsync();

            return examEntityEntry.Entity;
        }
    }
}
