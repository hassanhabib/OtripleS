// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
            TeacherContact teacherContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherContact> teacherContactEntityEntry =
                await broker.TeacherContacts.AddAsync(entity: teacherContact);

            await broker.SaveChangesAsync();

            return teacherContactEntityEntry.Entity;
        }

        public IQueryable<TeacherContact> SelectAllTeacherContacts() =>
            this.TeacherContacts;

        public async ValueTask<TeacherContact> SelectTeacherContactByIdAsync(
            Guid teacherId,
            Guid contactId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.TeacherContacts.FindAsync(teacherId, contactId);
        }

        public async ValueTask<TeacherContact> UpdateTeacherContactAsync(
            TeacherContact teacherContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherContact> teacherContactEntityEntry =
                broker.TeacherContacts.Update(entity: teacherContact);

            await broker.SaveChangesAsync();

            return teacherContactEntityEntry.Entity;
        }

        public async ValueTask<TeacherContact> DeleteTeacherContactAsync(
            TeacherContact teacherContact)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<TeacherContact> teacherContactEntityEntry =
                broker.TeacherContacts.Remove(entity: teacherContact);

            await broker.SaveChangesAsync();

            return teacherContactEntityEntry.Entity;
        }
    }
}
