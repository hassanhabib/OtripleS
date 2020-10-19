// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<StudentContact> StudentContacts { get; set; }

        public async ValueTask<StudentContact> InsertStudentContactAsync(
            StudentContact StudentContact)
        {
            EntityEntry<StudentContact> StudentContactEntityEntry =
                await this.StudentContacts.AddAsync(StudentContact);

            await this.SaveChangesAsync();

            return StudentContactEntityEntry.Entity;
        }

        public IQueryable<StudentContact> SelectAllStudentContacts() =>
            this.StudentContacts.AsQueryable();

        public async ValueTask<StudentContact> SelectStudentContactByIdAsync(
            Guid studentId,
            Guid contactId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.StudentContacts.FindAsync(studentId, contactId);
        }

        public async ValueTask<StudentContact> UpdateStudentContactAsync(
            StudentContact StudentContact)
        {
            EntityEntry<StudentContact> StudentContactEntityEntry =
                this.StudentContacts.Update(StudentContact);

            await this.SaveChangesAsync();

            return StudentContactEntityEntry.Entity;
        }

        public async ValueTask<StudentContact> DeleteStudentContactAsync(
            StudentContact StudentContact)
        {
            EntityEntry<StudentContact> StudentContactEntityEntry =
                this.StudentContacts.Remove(StudentContact);

            await this.SaveChangesAsync();

            return StudentContactEntityEntry.Entity;
        }
    }
}
