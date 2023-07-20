// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentExamFee> StudentExamFees { get; set; }

        public async ValueTask<StudentExamFee> InsertStudentExamFeeAsync(StudentExamFee studentExamFee)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentExamFee> studentExamFeeEntityEntry =
                await broker.StudentExamFees.AddAsync(entity: studentExamFee);

            await broker.SaveChangesAsync();

            return studentExamFeeEntityEntry.Entity;
        }

        public IQueryable<StudentExamFee> SelectAllStudentExamFees() => this.StudentExamFees;

        public async ValueTask<StudentExamFee> SelectStudentExamFeeByIdsAsync(
            Guid studentId,
            Guid examFeeId)
        {
            var broker = new StorageBroker(this.configuration);
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.StudentExamFees.FindAsync(studentId, examFeeId);
        }

        public async ValueTask<StudentExamFee> UpdateStudentExamFeeAsync(StudentExamFee studentExamFee)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentExamFee> studentExamFeeEntityEntry =
                broker.StudentExamFees.Update(entity: studentExamFee);

            await broker.SaveChangesAsync();

            return studentExamFeeEntityEntry.Entity;
        }

        public async ValueTask<StudentExamFee> DeleteStudentExamFeeAsync(StudentExamFee studentExamFee)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentExamFee> studentExamFeeEntityEntry =
                broker.StudentExamFees.Remove(entity: studentExamFee);

            await broker.SaveChangesAsync();

            return studentExamFeeEntityEntry.Entity;
        }
    }
}
