// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }

        public async ValueTask<StudentRegistration> InsertStudentRegistrationAsync(
            StudentRegistration studentRegistration)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentRegistration> studentRegistrationEntityEntry =
                await broker.StudentRegistrations.AddAsync(entity: studentRegistration);

            await broker.SaveChangesAsync();

            return studentRegistrationEntityEntry.Entity;
        }

        public IQueryable<StudentRegistration> SelectAllStudentRegistrations() =>
            this.StudentRegistrations;

        public async ValueTask<StudentRegistration> SelectStudentRegistrationByIdAsync(
            Guid studentId,
            Guid registrationId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.StudentRegistrations.FindAsync(studentId, registrationId);
        }

        public async ValueTask<StudentRegistration> UpdateStudentRegistrationAsync(
            StudentRegistration studentRegistration)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentRegistration> studentRegistrationEntityEntry =
                broker.StudentRegistrations.Update(entity: studentRegistration);

            await broker.SaveChangesAsync();

            return studentRegistrationEntityEntry.Entity;
        }

        public async ValueTask<StudentRegistration> DeleteStudentRegistrationAsync(
            StudentRegistration studentRegistration)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentRegistration> studentRegistrationEntityEntry =
                broker.StudentRegistrations.Remove(entity: studentRegistration);

            await broker.SaveChangesAsync();

            return studentRegistrationEntityEntry.Entity;
        }
    }
}
