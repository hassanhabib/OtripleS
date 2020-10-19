// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<StudentGuardian> StudentGuardians { get; set; }

        public async ValueTask<StudentGuardian> InsertStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            EntityEntry<StudentGuardian> studentGuardianEntityEntry =
                await this.StudentGuardians.AddAsync(studentGuardian);

            await this.SaveChangesAsync();

            return studentGuardianEntityEntry.Entity;
        }

        public IQueryable<StudentGuardian> SelectAllStudentGuardians() =>
            this.StudentGuardians.AsQueryable();

        public async ValueTask<StudentGuardian> SelectStudentGuardianByIdAsync(Guid studentId, Guid guardianId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.StudentGuardians.FindAsync(studentId, guardianId);
        }

        public async ValueTask<StudentGuardian> UpdateStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            EntityEntry<StudentGuardian> studentGuardianEntityEntry =
                this.StudentGuardians.Update(studentGuardian);

            await this.SaveChangesAsync();

            return studentGuardianEntityEntry.Entity;
        }

        public async ValueTask<StudentGuardian> DeleteStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            EntityEntry<StudentGuardian> studentGuardianEntityEntry =
                this.StudentGuardians.Remove(studentGuardian);

            await this.SaveChangesAsync();

            return studentGuardianEntityEntry.Entity;
        }
    }
}
