// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.TeacherContacts;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<TeacherContact> TeacherContacts { get; set; }

        public async ValueTask<TeacherContact> InsertTeacherContactAsync(
            TeacherContact TeacherContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherContact> TeacherContactEntityEntry =
                await broker.TeacherContacts.AddAsync(TeacherContact);

            await broker.SaveChangesAsync();

            return TeacherContactEntityEntry.Entity;
        }

        public IQueryable<TeacherContact> SelectAllTeacherContacts() =>
            this.TeacherContacts.AsQueryable();

        public async ValueTask<TeacherContact> SelectTeacherContactByIdAsync(
            Guid teacherId,
            Guid contactId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.TeacherContacts.FindAsync(teacherId, contactId);
        }

        public async ValueTask<TeacherContact> UpdateTeacherContactAsync(
            TeacherContact TeacherContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherContact> TeacherContactEntityEntry =
                broker.TeacherContacts.Update(TeacherContact);

            await broker.SaveChangesAsync();

            return TeacherContactEntityEntry.Entity;
        }

        public async ValueTask<TeacherContact> DeleteTeacherContactAsync(
            TeacherContact TeacherContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherContact> TeacherContactEntityEntry =
                broker.TeacherContacts.Remove(TeacherContact);

            await broker.SaveChangesAsync();

            return TeacherContactEntityEntry.Entity;
        }
    }
}
