// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentGuardian> StudentGuardians { get; set; }

        public async ValueTask<StudentGuardian> InsertStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentGuardian> studentGuardianEntityEntry =
                await broker.StudentGuardians.AddAsync(entity: studentGuardian);

            await broker.SaveChangesAsync();

            return studentGuardianEntityEntry.Entity;
        }

        public IQueryable<StudentGuardian> SelectAllStudentGuardians() => this.StudentGuardians;

        public async ValueTask<StudentGuardian> SelectStudentGuardianByIdAsync(Guid studentId, Guid guardianId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.StudentGuardians.FindAsync(studentId, guardianId);
        }

        public async ValueTask<StudentGuardian> UpdateStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentGuardian> studentGuardianEntityEntry =
                broker.StudentGuardians.Update(entity: studentGuardian);

            await broker.SaveChangesAsync();

            return studentGuardianEntityEntry.Entity;
        }

        public async ValueTask<StudentGuardian> DeleteStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentGuardian> studentGuardianEntityEntry =
                broker.StudentGuardians.Remove(entity: studentGuardian);

            await broker.SaveChangesAsync();

            return studentGuardianEntityEntry.Entity;
        }
    }
}
