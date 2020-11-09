// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Exam> Exams { get; set; }

        public async ValueTask<Exam> InsertExamAsync(Exam exam)
        {
            EntityEntry<Exam> examEntityEntry = await this.Exams.AddAsync(exam);
            await this.SaveChangesAsync();

            return examEntityEntry.Entity;
        }

        public IQueryable<Exam> SelectAllExams() => this.Exams.AsQueryable();

        public async ValueTask<Exam> SelectExamByIdAsync(Guid examId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Exams.FindAsync(examId);
        }

        public async ValueTask<Exam> UpdateExamAsync(Exam exam)
        {
            EntityEntry<Exam> examEntityEntry = this.Exams.Update(exam);
            await this.SaveChangesAsync();

            return examEntityEntry.Entity;
        }

        public async ValueTask<Exam> DeleteExamAsync(Exam exam)
        {
            EntityEntry<Exam> examEntityEntry = this.Exams.Remove(exam);
            await this.SaveChangesAsync();

            return examEntityEntry.Entity;
        }
    }
}
