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

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<TeacherContact> TeacherContacts { get; set; }

        public async ValueTask<TeacherContact> InsertTeacherContactAsync(
            TeacherContact TeacherContact)
        {
            EntityEntry<TeacherContact> TeacherContactEntityEntry =
                await this.TeacherContacts.AddAsync(TeacherContact);

            await this.SaveChangesAsync();

            return TeacherContactEntityEntry.Entity;
        }

        public IQueryable<TeacherContact> SelectAllTeacherContacts() =>
            this.TeacherContacts.AsQueryable();

        public async ValueTask<TeacherContact> SelectTeacherContactByIdAsync(
            Guid teacherId,
            Guid contactId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.TeacherContacts.FindAsync(teacherId, contactId);
        }

        public async ValueTask<TeacherContact> UpdateTeacherContactAsync(
            TeacherContact TeacherContact)
        {
            EntityEntry<TeacherContact> TeacherContactEntityEntry =
                this.TeacherContacts.Update(TeacherContact);

            await this.SaveChangesAsync();

            return TeacherContactEntityEntry.Entity;
        }

        public async ValueTask<TeacherContact> DeleteTeacherContactAsync(
            TeacherContact TeacherContact)
        {
            EntityEntry<TeacherContact> TeacherContactEntityEntry =
                this.TeacherContacts.Remove(TeacherContact);

            await this.SaveChangesAsync();

            return TeacherContactEntityEntry.Entity;
        }
    }
}
