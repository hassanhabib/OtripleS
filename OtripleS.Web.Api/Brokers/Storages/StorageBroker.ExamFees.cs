﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.ExamFees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ExamFee> ExamFees { get; set; }

        public async ValueTask<ExamFee> InsertExamFeeAsync(ExamFee examFee)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<ExamFee> examFeeEntityEntry = await broker.ExamFees.AddAsync(examFee);
            await broker.SaveChangesAsync();

            return examFeeEntityEntry.Entity;
        }

        public IQueryable<ExamFee> SelectAllExamFees() => ExamFees.AsQueryable();

        public async ValueTask<ExamFee> SelectExamFeeByIdAsync(Guid id)
        {
            using var broker = new StorageBroker(this.configuration);
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.ExamFees.FindAsync(id);
        }

        public async ValueTask<ExamFee> UpdateExamFeeAsync(ExamFee examFee)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<ExamFee> examFeeEntityEntry = broker.ExamFees.Update(examFee);
            await broker.SaveChangesAsync();

            return examFeeEntityEntry.Entity;
        }

        public async ValueTask<ExamFee> DeleteExamFeeAsync(ExamFee examFee)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<ExamFee> examFeeEntityEntry = broker.ExamFees.Remove(examFee);
            await broker.SaveChangesAsync();

            return examFeeEntityEntry.Entity;
        }
    }
}
